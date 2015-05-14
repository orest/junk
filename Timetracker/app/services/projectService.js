angular.module("timeTracker").factory('projectService', function ($http, $resource, globals) {

    var baseUrl = globals.urls.apiBase + "project/";
    var resource = $resource(baseUrl + ":id", { id: '@id' });
    var taskResource = $resource(globals.urls.apiBase + "tasks/" + ":id", { id: '@id' });

    function getProjects() {
        return resource.query();
    }


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
    };

    function addTask(task) {
        return taskResource.save(task);
    };

    function saveTask(task) {
        var $id = task.taskId;
        var r = $resource(globals.urls.apiBase + "tasks/:id", null, { 'update': { method: 'PUT' } });
        return r.update({ id: $id }, task);
    }


    return {
        getProject: getProject,
        getProjects: getProjects,
        save: save,
        addTask: addTask,
        saveTask: saveTask,
        update: update,
        remove: remove,
        findByDate: findByDate
    };
});

