Ext.define('CreditManager.UI.util.EnumMapper', {
    singleton: true,

    creditType: {
        1: 'Quick Credit',
        2: 'Vehicle Loan',
        3: 'Installment'
    },

    status: {
        1: 'Pending',
        2: 'Sent',
        3: 'Approved',
        4: 'Rejected',
        5: 'Cancelled'
    },

    getComboStore: function (enumObj) {
        return Object.keys(enumObj).map(key => [parseInt(key), enumObj[key]]);
    }
});