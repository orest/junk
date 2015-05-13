angular.module("timeTracker").factory('projectService', function ($http, $resource, globals) {

    var baseUrl = globals.urls.apiBase + "project/";
    var resource = $resource(baseUrl + ":id", { id: '@id' });

    function getProjects() {
        return resource.query();
    }

    //function getEntriesObj() {
    //    return $http.get(baseUrl);
    //}

    function findByDate(startDate, endDate) {
        return resource.query({ endDate: endDate, startDate: startDate });
    }

    function getProject(entryId) {
        return resource.get({ id: entryId });
    };

    function save(project) {
        return resource.save(project);
    };

    function remove(project) {
        return resource.delete({ id: project.Id });
    };

    function update(project) {
        var r = $resource(baseUrl, null, { 'update': { method: 'PUT' } });
        return r.update(project);
    }


    return {
        getProject: getProject,
        getProjects: getProjects,
        save: save,
        update: update,
        remove: remove,
        findByDate: findByDate
    };
});

