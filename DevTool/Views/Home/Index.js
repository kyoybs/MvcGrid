

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
            CategoryEditorData.CurrentCategoryName = this.model.Name;
            CategoryEditorData.CurrentCategoryId = this.model.Id;
            CategoryEditorData.Category = this.model;
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
 
var VueCategory;

$(function () { 
    // boot up the demo
    var categories = new Vue({
        el: '#CategoriesTree',
        data: {
            treeData: CategoryTreeData
        }
    })

    VueCategory = new Vue({
        el: '#Category',
        data: CategoryEditorData,
        methods: {            
            saveCtgName: function () {
                var pel = $(this.$el).getVueEl("CurrentCategoryName").parent();
                if (this.CurrentCategoryId == 0) { 
                    pel.showError("Please selected a category.");
                    return;
                } else {
                    pel.hideError();
                    var url = $(this.$el).attr("data-url-update");
                    var model = this;
                    $.post(url, { CategoryId: this.CurrentCategoryId, CategoryName: this.CurrentCategoryName }, function () {
                        model.Category.Name = model.CurrentCategoryName;
                        jq.showMsg("Save successfully.");
                    })
                }
            }
        }
    })
     
})

