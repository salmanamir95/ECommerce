import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DataStorageService {

  constructor() { }

  storeCartData(data : any){
    let cartData= JSON.stringify(data);
    localStorage.setItem('cart-data', cartData);
  }

  getCartData() {
    try {
      let getData = localStorage.getItem('cart-data');
      return getData ? JSON.parse(getData) : null;
    } catch (error) {
      console.error("Error parsing cart data from localStorage", error);
      return null; // Optionally return an empty cart or a default value
    }
  }
  
}
