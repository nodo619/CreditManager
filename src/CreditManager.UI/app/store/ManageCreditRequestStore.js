Ext.define('CreditManager.UI.store.ManageCreditRequestStore', {
    extend: 'Ext.data.Store',
    alias: 'store.managecreditrequeststore',

    requires: [
        'CreditManager.UI.model.CreditRequest',
        'CreditManager.UI.service.ApiService'
    ],

    model: 'CreditManager.UI.model.CreditRequest',
    autoLoad: true,
    pageSize: 10,

    load: function (options = {}) {
        const me = this;

        const page = (options.page || 1);
        const pageSize = me.getPageSize();

        CreditManager.UI.service.ApiService.get(
            `CreditRequests?PageNumber=${page}&PageSize=${pageSize}`
        ).then(result => {
            me.loadData(result.items);
            me.totalCount = result.totalCount;
            me.fireEvent('load', me, result.items, true);
        }).catch(err => {
            Ext.Msg.alert('Error', err.message);
        });
    }
});