angular.module("timeTracker").factory('globals', function () {

    var urls = {
        apiBase: 'http://localhost:6709/api/'
    };

    return {
        urls: urls
    }
});