 
//category edit date 

var veCategory;
var veTree;
var veCategoryFields;
var veField;

commands = {
    CategorySelected: "CategorySelected",
    CategoryUpdated: "CategoryUpdated",
    CategoryAdded: "CategoryAdded",
    CategoryDeleted:"CategoryDeleted"
};

$(function () {
     
    //boot up category tree.
    veTree = new Vue({
        el: '#CategoriesTree',
        data: {
            treeData: {},
            SelectedNode: null
        },
        methods: {
            nodeClick: function (node) {
                veTree.SelectedNode = node;
                veTree.broadcast(commands.CategorySelected, node.Entity);
            },
            deleteCtg:function(){ 
                if (veTree.SelectedNode == veTree.treeData) {
                    jq.showMsg("Cannot delete root category.");
                    return;
                }
                if (veTree.SelectedNode.Children.length > 0) {
                    jq.showMsg("Cannot delete a category with subcategories.");
                    return;
                }
                if (!confirm("Confirm to Delete Category " + this.SelectedNode.Name + "?"))
                    return;
                var url = $(this.$el).attr("data-url-delete");
                $.post(url, { CategoryId: this.SelectedNode.Id }, function () {
                    veTree.removeCategoryNode(veTree.treeData, veTree.SelectedNode);
                    veTree.SelectedNode = null;
                    veTree.broadcast(commands.CategoryDeleted, null);
                })
            },
            removeCategoryNode:function (parentCategory, category) {
                if (typeof (parentCategory) == "undefined" )
                    return false;

                $.each(parentCategory.Children, function (index, item) { 
                    if (category == item) { 
                        parentCategory.Children.splice(index,1);
                        return true;
                    }
                    if (veTree.removeCategoryNode(item, category))
                        return true;
                })
                return false;
            }
        }
    })
    
    veTree.listening(function (command, category) { 
        switch (command) {
            case commands.CategoryUpdated: 
                veTree.SelectedNode.Name = category.CategoryName;
                veTree.SelectedNode.Entity = category;
                break;
            case commands.CategoryAdded:
                veTree.$refs.tree.addChild(category.CategoryId, category.CategoryName, category); 
                //veTree.SelectedNode.Children.push(childNode);
                break;
            case commands.CategoryDeleted: 
                veTree.deleteCtg(category);
                break;
            default: 
        } 
    })
     

    //boot up category edit
    veCategory = new Vue({
        el: '#Category',
        data: {
            Category0: {},
            Category: {},
            ChildCategoryName:""
        },
        computed: {
            Disabled: function () { return this.Category0 == null; },
            OriginCategoryName: function () {
                return this.Category0 == null || typeof (this.Category0.CategoryId) == "undefined"
                    ? "Unselect"
                    : "#" + this.Category0.CategoryId + " " + this.Category0.CategoryName;
            }
        }, 
        methods: {
            initData:function(category){
                this.Category0 = category;
                this.Category = jq.cloneEntity(category);
            },

            saveCtgName: function () {
                var pel = $(this.$el).getVueEl("Category.CategoryName").parent();
                if (this.Category.CategoryName == "") {
                    pel.showError("Required.");
                    return;
                }  
                pel.hideError();

                var url = $(this.$el).attr("data-url-update"); 
                $.post(url, this.Category , function (data) { 
                    veCategory.Category0 =jq.cloneEntity( veCategory.Category );
                    veCategory.broadcast(commands.CategoryUpdated, veCategory.Category);
                    jq.showMsg("Save successfully.");
                })
                 
            },

            addChildCtg: function () { 
                var pel = $(this.$el).getVueEl("ChildCategoryName").parent();
                if (this.ChildCategoryName == "") {
                    pel.showError("Required.");
                    return;
                } 
                pel.hideError();

                var url = $(this.$el).attr("data-url-add");
                var newCtg = { ParentId: this.Category0.CategoryId, CategoryName: this.ChildCategoryName };
                $.post(url, newCtg, function (newCategory) {
                    //veCategory.TreeNodeModel.Children.push(newTreeNode);
                    veCategory.broadcast(commands.CategoryAdded, newCategory);
                    veCategory.ChildCategoryName = "";
                    jq.showMsg("Add successfully.");
                })
                 
            },

            deleteCtg: function () {
                veCategory.broadcast(commands.CategoryDeleted, veCategory.Category);
            },

            clear:function(){ 
                veCategory.Category = {};
                veCategory.Category0 = {};
            } 
        }
    })

    veCategory.listening( function (command, data) {
        switch (command) {
            case commands.CategorySelected:
                veCategory.initData(data);
                break;
            case commands.CategoryDeleted: 
                veCategory.clear();
                jq.showMsg("Delete successfully.");
                break;
            default: 
        } 
    })


    //boot up category fields grid 
    veCategoryFields = new Vue({
        el: '#elCategoryFields',
        data: {
            searchQuery: '',
            gridColumns: [{ FieldName: 'FieldId', FieldLabel: '#', Align: 'right' }
                , { FieldName: 'FieldName', FieldLabel: 'Field Name', Align: 'left' } 
                , { FieldName: 'FieldLabel', FieldLabel: 'FieldLabel', Align: 'left' }
                , { FieldName: 'ControlIndex', FieldLabel: 'Order' }
            ],
            gridData: [],
            Category0: {},
            GeneratedText: ""
        },
        computed: {
            Disabled: function () { return this.Category0.CategoryId == 0; }
        },
        methods: {
            initData: function (category) {
                this.Category0 = category;
                this.GeneratedText = ""; 
                var url = $(this.$el).attr("data-url-fields") + "?categoryId=" + category.CategoryId;;

                $.post(url, function (datas) {
                    veCategoryFields.gridData = datas;
                });
            },
            openAddFields: function () {
                var url = $(this.$el).attr("data-url-select") ;
                jq.popWindow("Select Fields", url, { CategoryId: this.Category0.CategoryId, MainTable: this.Category0.MainTable }, function (command, data) {
                    if (command == "select") {
                        veCategoryFields.gridData.push(data);
                        return;
                    }
                });
            },
            select: function (index, entity) {
                veField.Field = entity;
                veField.CategoryId = veCategoryFields.Category0.CategoryId;
                veField.FieldLabel = entity.FieldLabel;
                veField.ControlIndex = entity.ControlIndex;
                veField.EntityProperty = entity.EntityProperty;
            },
            generateView: function () { 
                var url = $(this.$el).attr("data-url-genereate");
                $.post(url, { categoryId: this.Category0.CategoryId, type: "View" }, function (data) {
                    veCategoryFields.GeneratedText = data;
                })
            }
            ,
            generateEntity: function () {
                var url = $(this.$el).attr("data-url-genereate");
                $.post(url, { categoryId: this.Category0.CategoryId, type: "Entity" }, function (data) {
                    veCategoryFields.GeneratedText = data;
                })
            }
            ,
            generateSql: function () {
                var url = $(this.$el).attr("data-url-genereate");
                $.post(url, { categoryId: this.Category0.CategoryId, type: "sql" }, function (data) {
                    veCategoryFields.GeneratedText = data;
                })
            }
            ,
            generateKiml: function () {
                var url = $(this.$el).attr("data-url-genereate");
                $.post(url, { categoryId: this.Category0.CategoryId, type: "Kiml" }, function (data) {
                    veCategoryFields.GeneratedText = data;
                })
            }
            ,
            generateHtml: function () {
                var url = $(this.$el).attr("data-url-genereate");
                $.post(url, { categoryId: this.Category0.CategoryId, type: "Html" }, function (data) {
                    veCategoryFields.GeneratedText = data;
                })
            }
        }
    }) // End CategoryFields

    veCategoryFields.listening(function (command, data) {
        switch (command) {
            case commands.CategorySelected:
                veCategoryFields.initData(data);
                break;
            default:
        }
    })

    veField = new Vue({
        el: '#FieldInfoArea',
        data: {
            Field: null, CategoryId: 0, FieldLabel: "", ControlIndex: "", EntityProperty: "",
            ControlTypeId:0, ControlTypes:[]
        },
        methods: {
            save: function () {
                var url = $(this.$el).attr("data-url-save");
                var field = veField.Field;
                field.CategoryId = this.CategoryId;
                field.FieldLabel = this.FieldLabel;
                field.EntityProperty = this.EntityProperty;
                field.ControlIndex = this.ControlIndex;
                $.post(url, field, function (data) {
                    veField.Field.FieldLabel = veField.FieldLabel;
                    veField.Field.ControlIndex = veField.ControlIndex;
                    veField.Field.EntityProperty = veField.EntityProperty;
                    jq.showMsg("Field save successfully.");
                })
            }
        }
    }); // end Field

})



