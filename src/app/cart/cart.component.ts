import { Component, OnInit } from '@angular/core';
import { DataStorageService } from '../service/data-storage.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {

  constructor(private dataStorage: DataStorageService) { }

  getCartData: any;
  storeCartArray: any = [];
  totalAmount: number = 0;
  totalCartData: number = 0;
  ngOnInit(): void {
    this.getCartData = this.dataStorage.getCartData();
    this.getCartData.filter(
      (ele: any) => {
        this.totalAmount = ele.price + this.totalAmount;
        this.totalCartData++;
      }
    );
  }

  removeCart(data: any): void {
    this.totalAmount=0;
    this.totalCartData=0;
    localStorage.removeItem('cart-data');
    this.storeCartArray = [];
    this.getCartData.filter(
      (ele: any) => {
        if (ele.id != data.id) {
          this.storeCartArray.push(ele);
          this.totalAmount = ele.price + this.totalAmount;
          this.totalCartData++;
        }
      }
    );
    this.dataStorage.storeCartData(this.storeCartArray);
    this.getCartData = this.dataStorage.getCartData();
  }

  plusMinusCount(data: any, thing : string)
  {
    this.storeCartArray = [];
    this.totalAmount=0;
    var plusMinusVal= data.plusMinusCounter;
    if (thing == 'plus'){
      plusMinusVal= plusMinusVal + 1;
      this.getCartData.filter(
        (ele:any) => {
          if (ele.id == data.id)
            ele['plusMinusCounter']= plusMinusVal;
          this.totalAmount= ele.price * ele.plusMinusCounter + this.totalAmount; 
        }
        
      );
    }
    else if (thing == 'minus'){
      plusMinusVal= plusMinusVal - 1;
      this.getCartData.filter(
        (ele:any) => {
          if (ele.id == data.id)
            ele['plusMinusCounter']= plusMinusVal;
          this.totalAmount= ele.price * ele.plusMinusCounter + this.totalAmount; 
        }
      );
    }
    this.storeCartArray = this.getCartData;
    this.dataStorage.storeCartData(this.storeCartArray);
    this.getCartData= this.dataStorage.getCartData();
  }

}
