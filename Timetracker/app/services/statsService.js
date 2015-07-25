angular.module("timeTracker").factory('statsService', function ($http, $resource, globals) {

    var baseUrl = globals.urls.apiBase + "stats/";
    var resource = $resource(baseUrl + "/?action=:action&filter=:filter", { action: '@action' });

    function get(action,filter) {
        return resource.query({ action: action, filter: filter });
    };

    return {
        get: get
    };
});

