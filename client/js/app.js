(function(){
    'use strict';

    const URL = 'http://localhost:5000/neural/';

    const caracteres = [' ','9','8','7','6','5','4','3','2','1','0','Z','Y','Z','W','V','U','T','S','R','Q','P','O','N','M','L','K','J','I','H','G','F','E','D','C','B','A'];

    /**
     * Realiza o treino da rede neural
     */
    function train()
    {
        
        const serial = $('#form').serialize();

        const file = $('#train-file').get(0).files[0];

        if(!file){

            alert('Selecione o arquivo de treinamento!');
        }else{

            $('#train').hide();
            $('#train-load').show();
            
            const reader = new FileReader();
            reader.readAsText(file);
            $(reader).on('load', function(res){
                $.ajax({
                    url : URL+'train?'+serial,
                    type : 'POST',
                    headers: {
                        "Content-Type" : "application/json; charset=utf-8"
                    },
                    data: res.target.result,
                    success: () => {
                        $('#train-load').hide();
                        $('#train').show();
                    }
                });
            });
        }
        
    }


    /**
     * 
     */
    function test()
    {
        const file = $('#test-file').get(0).files[0];

        if(!file){
            alert('Selecione o arquivo de teste!');
        }else{

            $('#test').hide();
            $('#test-load').show();
            
            const reader = new FileReader();
            reader.readAsText(file);
            $(reader).on('load', function(res){
                $.ajax({
                    url : URL+'test',
                    type : 'POST',
                    headers: {
                        "Content-Type" : "application/json; charset=utf-8"
                    },
                    data: res.target.result,
                    success: (res) => {


                        $('#acuracy').html(res.acuracia.toFixed(3));
                        $('#error').html(res.erro.toFixed(3));

                        geraMatriz(res);
                        geraTabela(res);



                        $('#test-load').hide();
                        $('#test').show();
                    }
                });
            });
        }
    }

    function geraMatriz(data)
    {
        let html = '<table>';
        html += '<tr><th></th>';
        for(let i=0; i< caracteres.length; i++){
            html += '<th><b>'+caracteres[i]+'</b></th>';
        }
        html += '<th class="text-primary">FN</th><th class="text-primary">TN</th></tr>';

        for(let i=0; i < data.confusao.length; i++){
            html += '<tr>';
            html += '<th>'+caracteres[i]+'</th>';
            for(let j=0; j < data.confusao[i].length; j++){
                html += '<td';
                if(i==j){
                    html += ' class="diagonal"';
                }
                html += '><b>'+data.confusao[i][j]+'</b></td>';
            }
            html += '<td>'+data.fn[i]+'</td>';
            html += '<td>'+(data.tn - data.confusao[i][i])+'</td>';
            html += '</tr>';
        }
        
        html += '<tr><th class="text-primary">FP</th>';
        for(let i=0; i< caracteres.length; i++){
            html += '<td>'+data.fp[i]+'</td>';
        }
        html += '<td></td><td></td></tr>';
        
        html += '</table>';

        $('#matriz').html(html);
    }


    function geraTabela(res)
    {
        let html = '<table>';
        html += '<tr><th></th><th>Precisão</th><th>Sensitividade</th><th>Especificidade</th></tr>';
        for(let i=0; i < caracteres.length; i++){
            html += '<tr><th>'+caracteres[i]+'</th>';
            html += '<td>'+res.precision[i].toFixed(2)+'</td>';
            html += '<td>'+res.recall[i].toFixed(2)+'</td>';
            html += '<td>'+res.specificity[i].toFixed(2)+'</td></tr>';
        }
        html += '</table>';

        $('#tabela').html(html);
    }

    $(document).ready(function() {

        $('#train-load').hide();
        $('#test-load').hide();

        $('#train').click(train);
        $('#test').click(test);

    });


})();