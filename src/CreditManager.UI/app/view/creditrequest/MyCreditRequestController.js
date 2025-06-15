Ext.define('CreditManager.UI.view.creditrequest.MyCreditRequestController', {
    extend: 'Ext.app.ViewController',
    alias: 'controller.mycreditrequest',

    onViewClick: function (grid, rowIndex, colIndex) {
        const record = grid.getStore().getAt(rowIndex);
        console.log('View clicked for:', record.data);
    },

    onEditClick: function (grid, rowIndex, colIndex) {
        const record = grid.getStore().getAt(rowIndex);
        console.log('Edit clicked for:', record.data);
    },

    onDeleteClick: function (grid, rowIndex, colIndex) {
        const record = grid.getStore().getAt(rowIndex);
        console.log('Delete clicked for:', record.data);
    }
});