angular.module("timeTracker").factory('models', function () {

    var projectCommand = function (id, action, actionDetails) {
        this.projectId = id;
        this.action = action;
        this.actionDetails = actionDetails;
    }

    var todo = function (id, title) {
        this.id = id;
        this.title = title;
        this.isCompleted = false;
        this.Priority = 5;
    }


    var task = function (id, title, projectId) {
        this.taskId = id;
        this.priority = 1;
        this.title = title;
        this.statusCd = "todo";
        this.projectId = projectId;
        this.notes = "";
        this.statuses = [
            { code: "todo", description: "ToDo" },
            { code: "actv", description: "Active" },            
            { code: "cmptd", description: "Completed" }
        ];
    }
    return {
        Task: task,
        ProjectCommand: projectCommand,
        Todo: todo
    }
});