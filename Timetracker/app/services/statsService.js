angular.module("timeTracker").factory('statsService', function ($http, $resource, globals) {

    var baseUrl = globals.urls.apiBase + "stats/";
    var resource = $resource(baseUrl + "/?action=:action", { action: '@action' });

    function get(action) {
        return resource.query({ action: action });
    };

    return {
        get: get
    };
});

