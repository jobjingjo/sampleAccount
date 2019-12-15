// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
(function () {
    "use strict";
    var app = angular.module('myApp', []);
    app.run(function ($rootScope) {   
        $rootScope.balance = "mock";
        $rootScope.errorMessage = "mock";
        $rootScope.error = false;
    });
}());

(function () {
    "use strict";

    angular.module('myApp').filter('transactionType', function () {

        return function (data) {
            if (data === 0) {
                return "Deposit";
            }
            if (data === 1) {
                return "Withdraw";
            }
            return data;
        }
    });
})();


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
                        deferred.reject("Not enough balance");
                    }
                },
                    function myError(response) {
                        deferred.reject("Please try again");
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
                        deferred.reject("Account not found");
                    }
                },
                    function myError(response) {
                        deferred.reject("Please try again");
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
                        deferred.reject("Can not Transfer");
                    }
                },
                    function myError(response) {
                        deferred.reject("Please try again");
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
                        deferred.reject(response.data);
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
    function withdrawController($rootScope, $scope, accountService, $timeout, $window) {

         $scope.doWithdraw = function () {
             accountService.withdraw($scope.amount).then(x => {
                 $rootScope.balance = x;
                 $rootScope.error = false;
                 console.log(x);                 
                 $('.toast').toast('show');

                 $timeout(function () { $window.location.href = 'https://localhost:44318/Account/Index'; }, 1000);

             }).catch(err => {
                 $rootScope.error = true;
                 $rootScope.errorMessage = err;
                 $('.toast').toast('show');
                 console.log(err);
             });
         };
    }
    withdrawController.$inject = ['$rootScope','$scope', 'accountService', '$timeout', '$window'];
    angular.module('myApp').controller('withdrawController', withdrawController);

}());


(function () {
    "use strict";
    function depositController($rootScope,$scope, accountService, $timeout, $window) {

        $scope.doDeposit = function () {
            accountService.deposit($scope.amount).then(x => {
                $rootScope.balance = x;
                $rootScope.error = false;
                console.log(x);
                $('.toast').toast('show');

                $timeout(function () { $window.location.href = 'https://localhost:44318/Account/Index'; }, 1000);

            }).catch(err => {
                $rootScope.error = true;
                $rootScope.errorMessage = err;
                $('.toast').toast('show');
                console.log(err);
            });
        };


    }
    depositController.$inject = ['$rootScope','$scope', 'accountService', '$timeout', '$window'];
    angular.module('myApp').controller('depositController', depositController);

}());

(function () {
    "use strict";
    function transferController($rootScope, $scope, accountService, $window, $timeout) {

        $scope.doTransfer = function () {
            accountService.transfer($scope.amount, $scope.accountName).then(x => {
                $rootScope.balance = x;
                $rootScope.error = false;
                console.log(x);
                //$("#overlay").show();
                $('.toast-ok').toast('show');

                $timeout(function () { $window.location.href = 'https://localhost:44318/Account/Index'; }, 1000);

            }).catch(err => {
                $rootScope.error = true;
                $rootScope.errorMessage = err;
                $('.toast').toast('show');
                console.log(err);
            });
        };
    }
    transferController.$inject = ['$rootScope','$scope', 'accountService', '$window', '$timeout'];
    angular.module('myApp').controller('transferController', transferController);

}());

(function () {
    "use strict";
    function accountController($rootScope,$scope, accountService, $window, $timeout) {

        $scope.doSearch = function (page) {
            $("#overlay").show();
            accountService.search(page).then(x => {
                console.log(x);
                $rootScope.error = true;
                $scope.items = x.result;

                $scope.pageIndex = x.pageIndex;
                $scope.totalPages = x.totalPages;
                $scope.hasPreviousPage = x.hasPreviousPage;
                $scope.hasNextPage = x.hasNextPage;

                $("#overlay").hide();
            }).catch(err => {
                $rootScope.error = false;
                $rootScope.errorMessage = err;
                $('.toast').toast('show');
                $scope.items = [];
                console.log(err);
                $("#overlay").hide();
            });
        };



        $scope.doSearch();
    }
    accountController.$inject = ['$rootScope','$scope', 'accountService', '$window', '$timeout'];
    angular.module('myApp').controller('accountController', accountController);

}());