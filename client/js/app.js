(function(){
    'use strict';

    function geraMatriz(n){

        let html = '<table>';
        for(var i=0; i<n; i++){
            html += '<tr>';
            for(var j=0; j<n; j++){
                html += '<td';
                if(i==j){
                    html += ' class="diagonal"';
                }
                html += '>&nbsp;</td>';
            }
            html += '</tr>';
        }
        html += '</table>';

        $('#matriz').html(html);
    }

    $(document).ready(function() {
        geraMatriz(36);
    });


})();