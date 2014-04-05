(function() {
    'use strict';

    angular.module('app').directive('errSrc', [errSrc]);
    
    function errSrc () {
        var directive = {
            link: link
        };
        return directive;

        function link(scope, element, attrs) {
            element.bind('error', function() {
                element.attr('src', attrs.errSrc)
            });
        }
    }

})();