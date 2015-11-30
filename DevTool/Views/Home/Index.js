

// define the item component
Vue.component('treenode', {
    template: '#treeNode-template',
    props: {
        model: Object
    },
    data: function () {
        return {
            open: this.model.open|true
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
            CurrentCategory.Name = this.model.Name; 
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
        el: '#categories',
        data: {
            treeData: categoryData
        }
    })

    new Vue({
        el: '#Category',
        data: CurrentCategory
    })

    CurrentCategory.Name = "abc";
})

