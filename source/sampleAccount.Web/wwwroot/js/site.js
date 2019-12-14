// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
(function () {
    "use strict";
    var app = angular.module('myApp', []);

}());

(function () {
    "use strict";
    angular
        .module('myApp')
        .factory('accountService', accountService);

    accountService.$inject = ['$http', '$q', '$window', '$timeout'];

    function accountService($http, $q, $window, $timeout) {
        var baseUrl = "https://localhost:44318/Account/";

        var service = {
            withdraw: withdraw,
            deposit: deposit,
            transfer: transfer,
            search: search
        };

        function withdraw(amount) {
            var deferred = $q.defer();

            $http.post(baseUrl+ "/Withdraw", { Amount: amount })
                .then(function (response) {

                    if (response.status === 200) {
                        deferred.resolve(response.data);
                    } else {
                        deferred.reject(response);
                    }
                },
                    function myError(response) {
                        deferred.reject(response);
                    });

            return deferred.promise;

        }
        function deposit(amount) {
            var deferred = $q.defer();

            $http.post(baseUrl + "/Deposit", { Amount: amount })
                .then(function (response) {

                    if (response.status === 200) {
                        deferred.resolve(response.data);
                    } else {
                        deferred.reject(response);
                    }
                },
                    function myError(response) {
                        deferred.reject(response);
                    });

            return deferred.promise;

        }
        function transfer(amount, accountName) {
            var deferred = $q.defer();

            $http.post(baseUrl+ "/Transfer", { Amount: amount, TargetAccountNumber: accountName })
                .then(function (response) {

                    if (response.status === 200) {
                        deferred.resolve(response.data);
                    } else {
                        deferred.reject(response);
                    }
                },
                    function myError(response) {
                        deferred.reject(response);
                    });

            return deferred.promise;

        }
        function search(page) {
            var deferred = $q.defer();

            $http.get(baseUrl + "/Transactions", {
                params: { sortOrder: '', currentFilter: '', searchString: '', pageNumber: page }
            })
                .then(function (response) {

                    if (response.status === 200) {
                        deferred.resolve(response.data);
                    } else {
                        deferred.reject(response);
                    }
                },
                    function myError(response) {
                        deferred.reject(response);
                    });

            return deferred.promise;

        }
        return service;
    }
}());

(function () {
    "use strict";
     function withdrawController($scope, accountService, $timeout, $window) {

         $scope.doWithdraw = function () {
             accountService.withdraw($scope.amount).then(x => {
                 $scope.balance = x;
                 console.log(x);
                 //$("#overlay").show();
                 $('.toast').toast('show');

                 $timeout(function () { $window.location.href = 'https://localhost:44318/Account/Index'; }, 1000);

             }).catch(err => {
                 console.log(err);
             });
         };
    }
    withdrawController.$inject = ['$scope', 'accountService', '$timeout', '$window'];
    angular.module('myApp').controller('withdrawController', withdrawController);

}());


(function () {
    "use strict";
    function depositController($scope, accountService, $timeout, $window) {

        $scope.doDeposit = function () {
            accountService.deposit($scope.amount).then(x => {
                $scope.balance = x;
                console.log(x);
                //$("#overlay").show();
                $('.toast').toast('show');

                $timeout(function () { $window.location.href = 'https://localhost:44318/Account/Index'; }, 1000);

            }).catch(err => {
                console.log(err);
            });
        };


    }
    depositController.$inject = ['$scope', 'accountService', '$timeout', '$window'];
    angular.module('myApp').controller('depositController', depositController);

}());

(function () {
    "use strict";
    function transferController($scope, accountService, $window, $timeout) {

        $scope.doTransfer = function () {
            accountService.transfer($scope.amount, $scope.accountName).then(x => {
                $scope.balance = x;
                console.log(x);
                //$("#overlay").show();
                $('.toast').toast('show');

                $timeout(function () { $window.location.href = 'https://localhost:44318/Account/Index'; }, 1000);

            }).catch(err => {
                console.log(err);
            });
        };
    }
    transferController.$inject = ['$scope', 'accountService', '$window', '$timeout'];
    angular.module('myApp').controller('transferController', transferController);

}());

(function () {
    "use strict";
    function accountController($scope, accountService, $window, $timeout) {

        $scope.doSearch = function (page) {
            $("#overlay").show();
            accountService.search(page).then(x => {
                console.log(x);
                $scope.items = x.result;

                $scope.pageIndex = x.pageIndex;
                $scope.totalPages = x.totalPages;
                $scope.hasPreviousPage = x.hasPreviousPage;
                $scope.hasNextPage = x.hasNextPage;

                $("#overlay").hide();
            }).catch(err => {
                $scope.items = [];
                console.log(err);
                $("#overlay").hide();
            });
        };



        $scope.doSearch();
    }
    accountController.$inject = ['$scope', 'accountService', '$window', '$timeout'];
    angular.module('myApp').controller('accountController', accountController);

}());