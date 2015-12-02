//#sourceURL = SelectFields.js

var vmAllFields = [
          { FieldName: 'Chuck Norris', power: Infinity },
          { FieldName: 'Bruce Lee', power: 9000 },
          { FieldName: 'Jackie Chan', power: 7000 },
          { FieldName: 'Jet Li', power: 8000 }
];

// bootstrap the demo
var veAllFields = new Vue({
    el: '#AllFields',
    data: {
        searchQuery: '',
        gridColumns: [{ FieldName: 'FieldId', FieldLabel: '#' }, { FieldName: 'TableName', FieldLabel: 'Table Name' }, { FieldName: 'FieldName', FieldLabel: 'Field Name' }],
        gridData: vmAllFields
    },
    methods: {
        Search: function () {
            var url = $(this.$el).attr("data-url-search");
            $.post(url, { TableName: this.searchQuery, FieldName: this.searchQuery, FieldLabel: this.searchQuery }, function (datas) {
                veAllFields.gridData = datas;  // work
            });
        },
        select: function (entity) {
            alert(entity);
        }
    }
})