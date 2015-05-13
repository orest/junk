angular.module("timeTracker").factory('projectCommandService', function ($http, $resource, globals) {

    var baseUrl = globals.urls.apiBase + "ProjectCommand/";
    var resource = $resource(baseUrl + ":id", { id: '@id' });


    function save(project) {
        return resource.save(project);
    };

    return {

        process: save
    };
});

