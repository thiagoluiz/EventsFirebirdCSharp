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
No Exemplo acima, foi criado uma procedure onde é percorrido todos os itens data tabela TB_SAIDA a fim de atualizar o campo OBS para "OK" e cada vez que o item é percorrido irá disparar um evento "ATUALIZAPROGRESSO" que será utilizado pela aplicação C# para atualizar o progresso. No final, irá disparar mais um evento para sabermos que o processo finalizou

## Código autocomplete-photon.html
```html
<ion-item>
  <ion-label stacked>
    <span class="titulo_label">{{label}}</span>
  </ion-label>
  <ion-input #criterio type="text" class="iconRight" [placeholder]="placeholder" (keyup)="filtrar(criterio.value)" (ionBlur)="onBlur()" (ionFocus)="onFocus()" [(ngModel)]="localSelecionado" clearInput>
  </ion-input>
  <ion-icon [name]="icone" item-end color="primary" (click)="clickIcon()"></ion-icon>
</ion-item>

<ion-list no-lines *ngIf="localCollection?.length > 0 && digitando==true">
  <ion-item *ngFor="let local of localCollection" (click)="selecionar(local)">
    <b>{{local.properties.name}}</b>
    <p text-wrap>{{local.properties.street}}, {{local.properties.city}} - {{local.properties.state}}</p>
  </ion-item>
</ion-list>
```

## Código autocomplete-photon.scss
```css
autocomplete-photon {
    ion-list {
        ion-item{
            height: 50px;
            b{
                font-size: 13px;
            }
        }

    }
}
```

## Código autocomplete-photon.ts
```typescript
import { Component, Input, EventEmitter, Output } from '@angular/core';

import { Headers, Http, Response } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import { ToastController } from 'ionic-angular';

@Component({
  selector: 'autocomplete-photon',
  templateUrl: 'autocomplete-photon.html'
})
export class AutocompletePhotonComponent {

  @Input("label")
  label: string;

  //Expoe variavel do pai para o filho
  @Input("placeholder")
  placeholder: string;

  @Input("icon")
  icone: string = 'md-locate';

  localCollection: any[] = [];
  digitando: boolean = false;
  localSelecionado: string;

  //Expoe a variavel do filho para o pai
  @Output() onCoordinate = new EventEmitter<any>();
  coordinate = { latitude: 0, longitude: 0 };


  @Output() onClickIcon = new EventEmitter<any>();
  constructor(public http: Http, private toastCtrl: ToastController) {

  }

  listar(local: string): Promise<Response> {
    let url = 'http://photon.komoot.de/api/?q=' + local + '&lang=de&limit=5';

    //let headers = new Headers();
    //headers.append('Content-Type' , 'application/json');
    //headers.append('Authorization', 'Bearer ' + localStorage.getItem('token'));

    //return this.http.get(url, { headers: headers }).toPromise();
    return this.http.get(url).toPromise();
  }

  filtrar(criterio: string) {
    this.digitando = true;

    if (criterio.length < 3) {
      return;
    }

    this.listar(criterio).then((response: any) => {
      this.localCollection = response.json().features;

      if (this.localCollection.length < 1) {
        this.toastCtrl.create({
          message: 'Endereço não encontrado',
          duration: 3000
        }).present();
      }
    });
  }

  selecionar(local: any) {
    this.localSelecionado = local.properties.name;
    
    this.coordinate.latitude = local.geometry.coordinates[1];
    this.coordinate.longitude = local.geometry.coordinates[0];
    
    this.onCoordinate.emit(this.coordinate);
    
    this.digitando = false;
  }

  clickIcon(){
    this.onClickIcon.emit();
  }
  
  onFocus(){
    this.digitando=true;
  }

  onBlur(){
    setTimeout(() => {
      if (this.digitando==true){
        this.digitando=false;
      }
    }, 1500);
  }
}

```

## Adicione o componente no app.module.ts
Adicione o componente no declarations e entryComponents


## Como usar o componente na página home
Adicione essa linha no seu home.html
```html
<autocomplete-photon label="Origem" placeholder="Qual o local da solicitação?" (onCoordinate)="getCoordinateOrigem($event)" (onClickIcon)="showMapa('origem', 'Origem')"></autocomplete-photon>
```

Adicione no home.ts o código abaixo para obter a latitude e longitude
```typescript
getCoordinateOrigem(coordinate : any){
    this.latitudeOrigem = coordinate.latitude;
    this.longitudeOrigem = coordinate.longitude;

    if (this.latitudeOrigem != null && this.latitudeDestino != null) {
      this.loadUltimaPosicao();
    }
  }
```

## Veja um exemplo do componente funcionando
![](https://github.com/pauloanalista/AutocompletePlacesWithPhotonForIonic3/blob/master/PhotonComponentIonic3.gif?raw=true)
