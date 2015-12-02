


//category edit date
var CategoryEditData = {
    CurrentCategoryName: "", CurrentCategoryId: 0
    , ChildCategoryName: "", OriginCategoryName: "Unselected"
    , Category: null, TreeNodeModel: null
};

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
                if (CategoryEditData.TreeNodeModel != null)
                    CategoryEditData.TreeNodeModel.Selected = false;

                CategoryEditData.OriginCategoryName = veTree.ShowCategory(node.model);
                CategoryEditData.CurrentCategoryId = node.model.Id;
                CategoryEditData.TreeNodeModel = node;
                veCategoryFields.CurrentCategoryId = node.model.Id;
                var url = $(this.$el).attr("data-url-fields") + "?categoryId=" + node.model.Id;
                
                $.post(url, function (datas) {
                    veCategoryFields.gridData = datas;
                });
            }
        }
    })

    veTree.ShowCategory = function (category) {
        return "#" + category.Id + " " + category.Name;
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
        data: CategoryEditData,
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
                    var model = this;
                    $.post(url, { CategoryId: this.CurrentCategoryId, CategoryName: this.CurrentCategoryName }, function () {
                        model.TreeNodeModel.model.Name = model.CurrentCategoryName; 
                        model.OriginCategoryName = veTree.ShowCategory(model.TreeNodeModel.model);
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
                    var model = this;
                    $.post(url, { ParentId: this.CurrentCategoryId, CategoryName: this.ChildCategoryName }, function (newTreeNode) {
                        model.Category.Children.push(newTreeNode);
                        model.ChildCategoryName = "";
                        jq.showMsg("Add successfully.");
                    })
                }
            },
            deleteCtg: function () {
                if (CategoryTreeData == this.Category) {
                    jq.showMsg("Cannot delete root category.");
                    return;
                }
                if (this.Category.Children.length > 0) {
                    jq.showMsg("Cannot delete a category with subcategories.");
                    return;
                }
                if (!confirm("Confirm to Delete Category " + this.OriginCategoryName + "?"))
                    return;
                var url = $(this.$el).attr("data-url-delete");
                $.post(url, { CategoryId: this.CurrentCategoryId }, function () {
                    veTree.removeCategory(CategoryTreeData, CategoryEditData.Category);
                    CategoryEditData.CurrentCategoryId = 0;
                    CategoryEditData.CurrentCategoryName = "";
                    CategoryEditData.ChildCategoryName = "";
                    CategoryEditData.OriginCategoryName = "Unselected";
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
                , { FieldName: 'FieldName', FieldLabel: 'Field Name' }],
            gridData: [],
            CurrentCategoryId:0                     
        },
        computed: {
            Disabled: function () { return this.CurrentCategoryId == 0; }
        },
        methods: {
            openAddFields: function () {
                var url = $(this.$el).attr("data-url-select") + "?CategoryId=" + CategoryEditData.CurrentCategoryId;
                jq.popWindow("Select Fields", url, CategoryEditData.CurrentCategoryId);
            }
        }
    })

})



