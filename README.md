# Eventos do Firebird com C#
Exemplo de como monitorar Eventos do Firebird(POST_EVENT) utilizando C#

## Proposta
Criei esse exemplo utilizando o POST_EVENT(Nativo do Firebird) e mostrarei como criar um listener no C# para ficar escutando esses eventos. Podemos utilizar os Events para diversas finalidades, uma delas é por exemplo ao rodarmos uma procedure com consultas a operações pesadas e após concluir cada processo disparar um evento e mostrar no log.

Abaixo temos no site do próprio firebird uma documentação sobre o evento:
https://firebirdsql.org/file/documentation/reference_manuals/driver_manuals/odbc/html/fbodbc205-events.html



# [Events com C#](#)

## Criando Eventos no Firebird
```sql
CREATE OR ALTER PROCEDURE SP_DISPARA_EVENTOS
AS
DECLARE VARIABLE VID_SAIDA INTEGER = 0;
BEGIN
  FOR SELECT SD1.ID_SAIDA FROM TB_SAIDA_ITEM SD1
  INTO: VID_SAIDA
  DO
  BEGIN
    UPDATE TB_SAIDA_ITEM SI SET SI.OBS = 'OK' WHERE SI.ID_SAIDA = :VID_SAIDA;

    IN AUTONOMOUS TRANSACTION DO
      POST_EVENT 'ATUALIZAPROGRESSO';

  END  
  
  IN AUTONOMOUS TRANSACTION DO
    POST_EVENT 'PROCESSOCONCLUIDO';  
END
```
No Exemplo acima, foi criado uma procedure onde é percorrido todos os itens data tabela TB_SAIDA a fim de atualizar o campo OBS para "OK" e cada vez que o item é percorrido irá disparar um evento "ATUALIZAPROGRESSO" que será utilizado pela aplicação C# para atualizar o progresso. No final, irá disparar mais um evento para sabermos que o processo finalizou.

Obs: Coloquei o evento dentro de uma transação autônoma, sem essa clausula todas as notificações só seriam disparadas após o commit.

## Código C#
```html

```


## Veja um exemplo do componente funcionando
![](https://github.com/pauloanalista/AutocompletePlacesWithPhotonForIonic3/blob/master/PhotonComponentIonic3.gif?raw=true)
