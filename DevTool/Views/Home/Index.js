


//category edit date
var CategoryEditData = {
    CurrentCategoryName: "", CurrentCategoryId: 0
    , ChildCategoryName: "", OriginCategoryName: "Unselected"
    , Category: null, TreeNodeModel: null
};

var veCategory;
var veTree;

$(function () {


    // define the tree component
    Vue.component('treenode', {
        template: '#treeNode-template',
        props: {
            model: Object
        },
        data: function () {
            return {
                open: true,
                Selected: false
            }
        },
        computed: {
            isFolder: function () {
                return this.model.Children &&
                  this.model.Children.length
            }
        },
        methods: {
            click: function () {
                if (CategoryEditData.TreeNodeModel != null)
                    CategoryEditData.TreeNodeModel.Selected = false;
                this.Selected = true;
                CategoryEditData.OriginCategoryName = veTree.ShowCategory(this.model);
                CategoryEditData.CurrentCategoryId = this.model.Id;
                CategoryEditData.TreeNodeModel = this;

            },
            toggle: function () {
                if (this.isFolder) {
                    this.open = !this.open
                }
                return false;
            },
            addChild: function () {
                this.model.Children.push({
                    Name: 'new stuff'
                })
            }
        }
    })

    // register the grid component
    Vue.component('demo-grid', {
        template: '#grid-template',
        props: {
            data: Array,
            columns: Array,
            filterKey: String
        },
        data: function () {
            var sortOrders = {}
            $.each(this.columns, function (index, item) {
                sortOrders[item] = 1;
            });

            return {
                sortKey: '',
                sortOrders: sortOrders
            }
        },
        methods: {
            sortBy: function (key) {
                this.sortKey = key
                this.sortOrders[key] = this.sortOrders[key] * -1
            }
        }
    })

    //boot up category tree.
    veTree = new Vue({
        el: '#CategoriesTree',
        data: {
            treeData: CategoryTreeData
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
                        model.Category.Name = model.CurrentCategoryName;
                        model.OriginCategoryName = veTree.ShowCategory(this.model.Category);
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
    var veCategoryFields = new Vue({
        el: '#CategoryFields',
        data: {
            searchQuery: '',
            gridColumns: ['name', 'power'],
            gridData: [
              { name: 'Chuck Norris', power: Infinity },
              { name: 'Bruce Lee', power: 9000 },
              { name: 'Jackie Chan', power: 7000 },
              { name: 'Jet Li', power: 8000 }
            ]
        }
    
    })

    //ready end.
})



