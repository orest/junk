angular.module("timeTracker").factory('todoService', function ($http, $resource, globals) {

    var baseUrl = globals.urls.apiBase + "todo/";
    var resource = $resource(baseUrl + ":id", { id: '@id' });

    function getActiveTodos() {        
        return resource.query({ activeOnly: false });
    }

    function getAllTodos() {
        return resource.query();
    }


    function getTodo(id) {
        return resource.get({ id: id });
    };


    function update(todo) {
        var $id = todo.id;
        var r = $resource(globals.urls.apiBase + "todo/:id", null, { 'update': { method: 'PUT' } });
        return r.update({ id: $id }, todo);
    }

    function addTodo(todo) {
        return resource.save(todo);
    };

    return {
        getAllTodos: getAllTodos,
        getActiveTodos: getActiveTodos,
        getTodo: getTodo,        
        addTodo: addTodo,
        update: update
    };
});

