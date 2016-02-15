angular.module("timeTracker").factory('statsService', function ($http, $resource, globals) {

    var baseUrl = globals.urls.apiBase + "stats/";
    var resource = $resource(baseUrl + "/?action=:action&filter=:filter", { action: '@action' });

    function get(action, filter) {
        return resource.query({ action: action, filter: filter });
    };
    function getByDate(startDate, endDate) {
        var start = moment(startDate).format("MM/DD/YYYY");
        var end = moment(endDate).format("MM/DD/YYYY");;


        var r = $resource(baseUrl + "/?startDate=" + start + "&endDate=" + end);
        return r.query();
    };
    return {
        get: get,
        getByDate: getByDate
    };
});

