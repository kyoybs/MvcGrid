

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
        data: CategoryEditorData
    })
     
})

