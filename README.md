# S4E.ADO.DBHandler

Projeto de Seleção para a Empresa S4E

Foi criado a tabela de Associados com as seguintes definições: id - int, auto increment, primary key Nome - string, MaxLength(200)(nvarchar(200), not null Cpf - string, StringLength(11)(nvarchar(11), Unique, not null DataDeNascimento - Datatime2 - null

Foi Criado a Tabela de Empresas id - int, auto increment, primary key Nome - string, MaxLength(200)(nvarchar(200), not null Cnpj - string, StringLength(14)(nvarchar(14), Unique, not null

Foi criado a tabela AssociadoEmpresa, essa tabela faz o relacionamento de N para N da tabela Associados e Empresas AssociadoId - int , foreign key - Associado.id EmpresaId - int , foreign key - Empresa.id

Os Controllers da Api ficaram os seguintes: Primeiro para Associados o controler é: AssociadoController

AdicionaAssociado irá adicionar o associado atrávez do body em json adiciona o associado: { "nome": "nome do associado", "cpf": "CPF do associado", "dataDeNascimento": "Data", "empresasId": ["Array de int do relacionamento com as empresas"] } Caminho em Post: https://localhost:7073/Associado
RecuperAssociados, mostra todos os associados com suas relações , devolução é em json caminho em Get: https://localhost:7073/Associado
RecuperaAssociadosPorNome ira achar todos os associados com o mesmo nome com suas relações e irá retornar em json caminho em get: https://localhost:7073/Associado/nome/nome-sobrenome
RecuperaAssociadoPorId irá buscar apenas um associado com o id e retornara em json caminho get: https://localhost:7073/Associado/id/iddoassociado
RecuperaAssociadoPorCPF irá buscar apenas um associado com o cpf e retornara em json caminho get: https://localhost:7073/Associado/cpf/cpfdoassociado
AtualizaAssociado irá atualizar os dados do associado, busca pelo id e no body em json como no adicionar ira conter os dados para a alteração irá deletar todas as relações e adicionar as novas, caso CPF esteja vazio continuara o anterios, o mesmo vale para o nome se tiver vazio o campo de empresasId o associado ficará sem relação caminho post: https://localhost:7073/Associado/iddoassociado
DeletaAssociado irá deletar o associado e todas as suas relações caminho Delete: https://localhost:7073/Associado/iddoassociado
