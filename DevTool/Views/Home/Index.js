﻿


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
                if (!confirm("Confirm to Delete Category " + this.OriginCategoryName + "?"))
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
                var childNode = { Id: category.CategoryId, Name: category.CategoryName, Entity: category };
                veTree.SelectedNode.Children.push(childNode);
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
            CurrentCategoryName: "", CurrentCategoryId: 0,
            ChildCategoryName: "", Category: null, MainTable: ""
        },
        computed: {
            Disabled: function () { return this.CurrentCategoryId == 0; },
            OriginCategoryName: function () {
                return this.Category==null?"Unselect": "#" + this.Category.CategoryId + " " + this.Category.CategoryName; 
            }
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
                        veCategory.Category.CategoryName = veCategory.CurrentCategoryName;
                        veCategory.Category.MainTable = veCategory.MainTable;
                        veCategory.broadcast(commands.CategoryUpdated, veCategory.Category);
                        veCategoryFields.MainTable = veCategory.MainTable;
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
                    var newCtg = { ParentId: this.CurrentCategoryId, CategoryName: this.ChildCategoryName };
                    $.post(url, newCtg, function (newCategory) {
                        //veCategory.TreeNodeModel.Children.push(newTreeNode);
                        veCategory.broadcast(commands.CategoryAdded, newCategory);
                        veCategory.ChildCategoryName = "";
                        jq.showMsg("Add successfully.");
                    })
                }
            }, 
            deleteCtg: function () {
                veCategory.broadcast(commands.CategoryDeleted, veCategory.Category);
            },
            clear:function(){ 
                veCategory.CurrentCategoryId = 0;
                veCategory.CurrentCategoryName = "";
                veCategory.ChildCategoryName = "";
                veCategory.OriginCategoryName = "Unselected";
                veCategory.Category = null; 
            },
            selectCtg:function(category){

                //init edit info 
                veCategory.Category = category;
                veCategory.CurrentCategoryName = category.CategoryName;
                veCategory.CurrentCategoryId = category.CategoryId;
                veCategory.MainTable = category.MainTable; 

                //init category fields
                veCategoryFields.CurrentCategoryId = category.CategoryId;
                veCategoryFields.MainTable = category.MainTable;
                veCategoryFields.GeneratedText = "";
                var url = $(this.$el).attr("data-url-fields") + "?categoryId=" + category.CategoryId;;

                $.post(url, function (datas) {
                    veCategoryFields.gridData = datas;
                });
            }
        }
    })

    veCategory.listening( function (command, data) {
        switch (command) {
            case commands.CategorySelected:
                veCategory.selectCtg(data);
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
            CurrentCategoryId: 0,
            MainTable: "", GeneratedText: ""
        },
        computed: {
            Disabled: function () { return this.CurrentCategoryId == 0; }
        },
        methods: {
            openAddFields: function () {
                var url = $(this.$el).attr("data-url-select") ;
                jq.popWindow("Select Fields", url, { CategoryId: this.CurrentCategoryId, MainTable: this.MainTable }, function (command, data) {
                    if (command == "select") {
                        veCategoryFields.gridData.push(data);
                        return;
                    }
                });
            },
            select: function (index, entity) {
                veField.Field = entity;
                veField.CategoryId = veCategoryFields.CurrentCategoryId;
                veField.FieldLabel = entity.FieldLabel;
                veField.ControlIndex = entity.ControlIndex;
                veField.EntityProperty = entity.EntityProperty;
            },
            generateView: function () { 
                var url = $(this.$el).attr("data-url-genereate");
                $.post(url, { categoryId: this.CurrentCategoryId, type: "View" }, function (data) {
                    veCategoryFields.GeneratedText = data;
                })
            }
            ,
            generateEntity: function () {
                var url = $(this.$el).attr("data-url-genereate");
                $.post(url, { categoryId: this.CurrentCategoryId, type: "Entity" }, function (data) {
                    veCategoryFields.GeneratedText = data;
                })
            }
            ,
            generateSql: function () {
                var url = $(this.$el).attr("data-url-genereate");
                $.post(url, { categoryId: this.CurrentCategoryId, type: "sql" }, function (data) {
                    veCategoryFields.GeneratedText = data;
                })
            }
            ,
            generateKiml: function () {
                var url = $(this.$el).attr("data-url-genereate");
                $.post(url, { categoryId: this.CurrentCategoryId, type: "Kiml" }, function (data) {
                    veCategoryFields.GeneratedText = data;
                })
            }
            ,
            generateHtml: function () {
                var url = $(this.$el).attr("data-url-genereate");
                $.post(url, { categoryId: this.CurrentCategoryId, type: "Html" }, function (data) {
                    veCategoryFields.GeneratedText = data;
                })
            }
        }
    }) // End CategoryFields

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



