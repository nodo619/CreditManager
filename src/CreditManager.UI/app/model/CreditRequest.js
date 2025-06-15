Ext.define('CreditManager.UI.model.CreditRequest', {
    extend: 'Ext.data.Model',

    fields: [
        { name: 'id', type: 'string' },
        { name: 'customerId', type: 'string' },
        { name: 'amount', type: 'float' },
        { name: 'currencyCode', type: 'string' },
        { name: 'requestDate', type: 'date', dateFormat: 'c' },
        { name: 'periodYears', type: 'int' },
        { name: 'periodMonths', type: 'int' },
        { name: 'periodDays', type: 'int' },
        { name: 'creditType', type: 'int' },
        { name: 'status', type: 'int' },
        { name: 'comments', type: 'string' },
        { name: 'approvalDate', type: 'date', dateFormat: 'c' },
        { name: 'approvedBy', type: 'string' },
        { name: 'createdAt', type: 'date', dateFormat: 'c' },
        { name: 'lastModifiedAt', type: 'date', dateFormat: 'c' }
    ]
});