angular.module("timeTracker").controller("ProjectController",
    function ($scope, projectService, projectCommandService, todoService, $interval, globals, models, _, statsService) {
        var timer;
        $scope.projects = [];
        $scope.stopMessage = "";
        var t = new models.Task();
        $scope.taskStatuses = t.statuses;
        $scope.todoState = "ACTV";
        $scope.weekTotal = 0;
        var todoPriority = [];

        var getTime = function (prj) {
            var prjCommand = { projectId: prj.project.projectId, action: "time", actionDetails: "" };
            $scope.elapsedTime = projectCommandService.process(prjCommand);
        },

        cancelTimer = function () {
            if (angular.isDefined(timer)) {
                $interval.cancel(timer);
                timer = undefined;
            }
        },

        startTimer= function(prj) {
            timer = $interval(function () {
                getTime(prj);
            }, globals.refreshInterval);
        }

        //refresh projects
        function refreshProjects() {
            projectService.getProjects().$promise.then(function (data) {
                $scope.projects = data;
                angular.forEach($scope.projects, function (item) {
                    if (item.hasActiveLog) {
                        getTime(item);
                        startTimer(item);
                    }
                });
            });

        }

        //Start buttons
        $scope.start = function (p) {
            var prjCommand = new models.ProjectCommand(p.project.projectId, "start");
            angular.forEach($scope.projects, function (item) { item.hasActiveLog = false; });
            p.hasActiveLog = true;
            projectCommandService.process(prjCommand);
            $scope.elapsedTime = { minutes: 0, hours: 0 };

            timer = $interval(function () {
                getTime(p);
            }, globals.refreshInterval);
        }
        //Restart
        $scope.restart = function (p) {
            var prjCommand = new models.ProjectCommand(p.project.projectId, "restart");
            projectCommandService.process(prjCommand);
            p.isPaused = false;
            timer = $interval(function () {
                getTime(p);
            }, globals.refreshInterval);
        }


        //stopping
        $scope.stopping=function(prj) {
            prj.stopping = true;
            cancelTimer();
        }

        //cancel stopping
        $scope.cancelStopping = function (prj) {
            prj.stopping = false;
            startTimer(prj);
        }
        // Stop buttons
        $scope.stop = function (p) {
            var details = { message: $scope.stopMessage, time: $scope.elapsedTime.minutes };

            var prjCommand = new models.ProjectCommand(p.project.projectId, "stop", JSON.stringify(details));
            p.hasActiveLog = false;
            p.stopping = false;
            $scope.stopMessage = "";
            projectCommandService.process(prjCommand);
            cancelTimer();
        }

        //Pause buttons
        $scope.pause = function (p) {
            var prjCommand = new models.ProjectCommand(p.project.projectId, "pause");
            p.isPaused = true;
            projectCommandService.process(prjCommand);
            cancelTimer();
        }

        //add task
        $scope.addTask = function (p) {
            var task = new models.Task(0, p.newTaskName, p.project.projectId);
            projectService.addTask(task).$promise.then(function () {
                refreshProjects();
            });
            p.newTaskName = "";
        }

        //updateTask task
        $scope.updateTask = function (task) {
            if (task.newTitle) {
                task.title = task.newTitle;
                projectService.saveTask(task);
            }
            task.editing = false;
        }

        //cancel task
        $scope.cancelTask = function (task) {
            task.editing = false;

        }
        $scope.changeToDo = function (todo) {
            todoService.update(todo);
        }
        //add todos
        $scope.addTodo = function () {
            var todo = new models.Todo(0, $scope.newTodo);
            todo.priority = $scope.todos.length;
            todoService.addTodo(todo);
            $scope.todos.push(todo);
            $scope.newTodo = "";
        }

        $scope.showActiveToDos = function () {
            todoService.getActiveTodos().$promise.then(function (data) {
                $scope.todos = data;
                _.each(data, function (item) {
                    todoPriority[item.id] = item.priority;
                });
            });
            $scope.todoState = "ACTV";
        }

        $scope.showAllToDos = function () {
            $scope.todos = todoService.getAllTodos();
            $scope.todoState = "ALL";
        }

        refreshProjects();
        $scope.showActiveToDos();
        $scope.todoSortable = {
            containment: "parent",//Dont let the user drag outside the parent
            cursor: "move",
            tolerance: "pointer",
            stop: function (e, ui) {
                var counter = 1;
                _.each($scope.todos, function (item) {
                    if (todoPriority[item.id] != counter) {
                        todoPriority[item.id] = counter;
                        item.priority = counter;
                        $scope.changeToDo(item);
                    }
                    counter++;
                });
            }
        };

        statsService.get('weeklyreport').$promise.then(function (data) {
            $scope.stat = data;
            var total = _.reduce($scope.stat, function (num, item) {
                return num + Number(item.elapsed);
            }, 0);
            $scope.weekTotal = total;
        });


    });
