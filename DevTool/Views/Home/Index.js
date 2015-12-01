

// define the item component
Vue.component('treenode', {
    template: '#treeNode-template',
    props: {
        model: Object
    },
    data: function () {
        return {
            open: true
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
            CategoryEditData.OriginCategoryName = "#"+this.model.Id + " " + this.model.Name;
            CategoryEditData.CurrentCategoryId = this.model.Id;
            CategoryEditData.Category = this.model;
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
  
var CategoryEditData = { CurrentCategoryName: "", CurrentCategoryId: 0, ChildCategoryName: "", OriginCategoryName: "Unselected", Category: {} };

$(function () { 
    // boot up the demo
    var categories = new Vue({
        el: '#CategoriesTree',
        data: {
            treeData: CategoryTreeData
        }
    })

    new Vue({
        el: '#Category',
        data: CategoryEditData,
        computed:{
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
                        model.OriginCategoryName = "#" + model.Category.Id + " " + model.Category.Name;;
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
                        jq.showMsg("Save successfully.");
                    })
                }
            },
            deleteCtg: function () {
                if (CategoryTreeData == this.Category) {
                    jq.showMsg("Cannot delete root category.");
                    return;
                }
                if (this.Category.Children.length >0) {
                    jq.showMsg("Cannot delete a category with subcategories.");
                    return;
                }
                if (!confirm("Confirm to Delete Category "  + this.OriginCategoryName + "?"))
                    return ;
                var url = $(this.$el).attr("data-url-delete");
                $.post(url, { CategoryId: this.CurrentCategoryId }, function () {
                    removeCategory(CategoryEditData.Category);
                    CategoryEditData.CurrentCategoryId = 0;
                    CategoryEditData.CurrentCategoryName = "";
                    CategoryEditData.OriginCategoryName = "Unselected"; 
                    jq.showMsg("Delete successfully.");
                })
            },
            removeTreeModel: function () {
                alert("Remove Tree");
            }

        }// end methods
    })
     
})

function removeCategory(parentCategory, category) {
    $.each(parentCategory.Children, function (index, item) {
        if (category == item) {
            category.Children.pop(item);
            return true;
        }
        if(removeCategory(item, category))
            return true; 
    })
    return false;
}

