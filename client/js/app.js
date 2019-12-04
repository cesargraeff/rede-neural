(function(){
    'use strict';

    const URL = 'http://localhost:5000/neural/';

    /**
     * Realiza o treino da rede neural
     */
    function train(){

        $('#load-treinando').show();
        const serial = $('#form').serialize();

        var reader = new FileReader();
        reader.readAsText($('#file-train').get(0).files[0]);

        $(reader).on('load', function(res){
            $.ajax({
                url : URL+'train?'+serial,
                type : 'POST',
                datatype: 'JSON',
                data: res.target.result,
                success: function(res){
                    $('#load-treinando').hide();
                }
            });
        });
    }

    function test(){

    }

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

        $('#load-treinando').hide();

        $('#train').click(function(){
            train();
        });

        geraMatriz(36);
    });


})();