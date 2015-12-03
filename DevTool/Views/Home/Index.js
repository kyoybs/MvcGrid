


//category edit date 

var veCategory;
var veTree;
var veCategoryFields;

$(function () {
     
    //boot up category tree.
    veTree = new Vue({
        el: '#CategoriesTree',
        data: {
            treeData: CategoryTreeData
        },
        methods: {
            nodeClick: function (node) {
                //init edit info
                veCategory.OriginCategoryName = veTree.ShowCategory(node);
                veCategory.CurrentCategoryName = node.Name;
                veCategory.CurrentCategoryId = node.Id;
                veCategory.MainTable = node.Entity.MainTable;
                veCategory.TreeNodeModel = node;

                //init category fields
                veCategoryFields.CurrentCategoryId = node.Id;
                veCategoryFields.MainTable = node.Entity.MainTable;
                var url = $(this.$el).attr("data-url-fields") + "?categoryId=" + node.Id;
                
                $.post(url, function (datas) {
                    veCategoryFields.gridData = datas;
                });
            }
        }
    })

    veTree.ShowCategory = function (node) {
        return "#" + node.Id + " " + node.Name;
    };

    veTree.removeCategory = function (parentCategory, category) {
        if (typeof (parentCategory) == "undefined" )
            return false;

        $.each(parentCategory.Children, function (index, item) { 
            if (category == item) { 
                parentCategory.Children.splice(index,1);
                return true;
            }
            if (veTree.removeCategory(item, category))
                return true;
        })
        return false;
    }

    //boot up category edit
    veCategory = new Vue({
        el: '#Category',
        data: {  CurrentCategoryName: "", CurrentCategoryId: 0
    , ChildCategoryName: "", OriginCategoryName: "Unselected"
    , Category: null, TreeNodeModel: null, MainTable:""
        },
        computed: {
            Disabled: function () { return this.CurrentCategoryId == 0; }
        },
        methods: {
            saveCtgName: function () {
                var pel = $(this.$el).getVueEl("CurrentCategoryName").parent();
                if (this.CurrentCategoryId == 0) {
                    pel.showError("Please selected a category.");
                    return;
                } else if (this.CurrentCategoryName == "") {
                    pel.showError("Required.");
                    return;
                } else {
                    pel.hideError();
                    var url = $(this.$el).attr("data-url-update"); 
                    $.post(url, { CategoryId: this.CurrentCategoryId, CategoryName: this.CurrentCategoryName, MainTable: this.MainTable }, function (data) { 
                        veCategory.TreeNodeModel.Name = veCategory.CurrentCategoryName;
                        veCategory.TreeNodeModel.Entity.CategoryName = veCategory.CurrentCategoryName;
                        veCategory.TreeNodeModel.Entity.MainTable = veCategory.MainTable;
                        veCategory.OriginCategoryName = veTree.ShowCategory(veCategory.TreeNodeModel);
                        
                        jq.showMsg("Save successfully.");
                    })
                }
            },
            addChildCtg: function () {
                //check current category
                var pel = $(this.$el).getVueEl("CurrentCategoryName").parent();
                if (this.CurrentCategoryId == 0) {
                    pel.showError("Please selected a category.");
                    return;
                }

                //check child  category and save.
                pel = $(this.$el).getVueEl("ChildCategoryName").parent();
                if (this.ChildCategoryName == "") {
                    pel.showError("Required.");
                    return;
                } else {
                    pel.hideError();
                    var url = $(this.$el).attr("data-url-add"); 
                    $.post(url, { ParentId: this.CurrentCategoryId, CategoryName: this.ChildCategoryName }, function (newTreeNode) {
                        veCategory.TreeNodeModel.Children.push(newTreeNode);
                        veCategory.ChildCategoryName = "";
                        jq.showMsg("Add successfully.");
                    })
                }
            },
            deleteCtg: function () {
                if (veTree.treeData == this.Category) {
                    jq.showMsg("Cannot delete root category.");
                    return;
                }
                if (this.TreeNodeModel.Children.length > 0) {
                    jq.showMsg("Cannot delete a category with subcategories.");
                    return;
                }
                if (!confirm("Confirm to Delete Category " + this.OriginCategoryName + "?"))
                    return;
                var url = $(this.$el).attr("data-url-delete");
                $.post(url, { CategoryId: this.CurrentCategoryId }, function () { 
                    veTree.removeCategory(veTree.treeData, veCategory.TreeNodeModel);
                    veCategory.CurrentCategoryId = 0;
                    veCategory.CurrentCategoryName = "";
                    veCategory.ChildCategoryName = "";
                    veCategory.OriginCategoryName = "Unselected";
                    jq.showMsg("Delete successfully.");
                })
            }

        }
    })


    //boot up category fields grid 
    veCategoryFields = new Vue({
        el: '#elCategoryFields',
        data: {
            searchQuery: '',
            gridColumns: [{ FieldName: 'FieldId', FieldLabel: '#' }
                , { FieldName: 'TableName', FieldLabel: 'Table Name' }
                , { FieldName: 'FieldName', FieldLabel: 'Field Name' }
                , { FieldName: 'ControlIndex', FieldLabel: 'Order' }
            ],
            gridData: [],
            CurrentCategoryId: 0,
            MainTable:""
        },
        computed: {
            Disabled: function () { return this.CurrentCategoryId == 0; }
        },
        methods: {
            openAddFields: function () {
                var url = $(this.$el).attr("data-url-select") ;
                jq.popWindow("Select Fields", url, { CategoryId: this.CurrentCategoryId,MainTable:this.MainTable }, function (action, data) {
                    veCategoryFields.gridData.push(data);
                });
            },
            select: function (index, entity) {
                veField.Field = entity;
                veField.CategoryId = veCategoryFields.CurrentCategoryId;
                veField.FieldLabel = entity.FieldLabel;
                veField.ControlIndex = entity.ControlIndex;
                veField.EntityProperty = entity.EntityProperty;
            }
        }
    }) // End CategoryFields

    veField = new Vue({
        el: '#FieldInfoArea',
        data: { Field: null, CategoryId: 0, FieldLabel: "", ControlIndex: "", EntityProperty:""},
        methods: {
            save: function () {
                var url = $(this.$el).attr("data-url-save");
                var field = veField.Field;
                field.CategoryId = this.CategoryId;
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



