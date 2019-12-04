# Rede Neural

## Instalação

Instale a SDK do .NET Core no link abaixo:

[Download .NET Core](https://dotnet.microsoft.com/download)

## Inicialização

Após a instalação do .NET Core utilize o comando abaixo para iniciar a API:

    dotnet run

## Utilização

Abra o arquivo index.html da pasta client/

Utilize os arquivos [train.json](train.json) e [test.json](test.json) para realizar a execução

ou

Utilize um REST Client para a execução:

Faça uma requisição POST para a URL: http://localhost:5000/neural/train?iteracoes=2000&intermediaria=20&aprendizado=0.5&momentum=1

No body da requisição envie a requisição de treino contida no arquivo [train.json](train.json) e mude os parâmetros presentes na query string

Faça uma requisição POST para a URL: http://localhost:5000/neural/test
No body da requisição envie a requisição de teste contida no arquivo [test.json](test.json)

O resultado irá aparecer no terminal.


## Código

A lógica da aplicação encontra-se na classe NeuralNetwork localizada na pasta Core/

Na pasta client/ são definidos os códigos do front-end

Na pasta Model/ são definidas as classes com os parâmetros de entrada/saída da API


## Extensões para .NET Core no VSCode

.NET Core Snippet Pack

Auto-Using for C#

C#