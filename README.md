# ServicoAgrupaPDF
Serviço SelfHost usando Topshelf que le uma pasta com vários PDFs e Agrupa todos em um arquivo único.  

# AutocompletePlacesWithPhotonForIonic3
Autocomplete de locais semelhante ao do Google Maps(Places) para ionic 3 usando Photon (Gratuito)

## Proposta
Criei este componente com a finalidade de ajudar empresas e profissionais autonomos ha não dependerem mais da Places API do Google Maps.
Depois da mudança da política de monetização da Google o custo para usar essa API ficou muito alto, estava pagando em média 7 mil reais, com este novo componente o custo foi zerado.

OBS: Quero deixar claro que a API da Google tem muito mais qualidade, muito mais estabelecimentos, mas para quem tiver desesperado essa é uma ótima alternativa.

![](http://photon.komoot.de/static/img/photon_logo.png)



# [Photon API](http://photon.komoot.de)

## Criando o componente para Ionic 3
```sh
ionic generate component AutocompletePhoton
```

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
