import { Component, OnInit } from '@angular/core';
import { GetDataService } from '../service/get-data.service';
import { ActivatedRoute, Router } from '@angular/router';
import { DataStorageService } from '../service/data-storage.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})
export class ProductDetailsComponent implements OnInit {

  getParamValue: any;
  getProductDetails: any;
  storeCartData: any = [];
  inCart: boolean = false;

  constructor(
    private getData: GetDataService, 
    private route: ActivatedRoute, 
    private dataStorage: DataStorageService, 
    private router: Router
  ) { }

  ngOnInit(): void {
    // Get the product ID from the URL
    this.getParamValue = this.route.snapshot.paramMap.get('id');
    console.log("Product ID:", this.getParamValue);

    // Load cart data from local storage
    this.storeCartData = this.dataStorage.getCartData() || [];
    console.log("Stored Cart Data:", this.storeCartData);

    // Ensure product data is available before filtering
    if (this.getData.productData != null) {
      this.getProductDetails = this.getData.productData.find((ele: any) => ele.id == this.getParamValue);
    }

    // Handle case where product is not found
    if (this.getProductDetails == null) {
      console.error("Product not found!");
    }

    // Check if the product is already in the cart
    if (this.storeCartData.length > 0) {
      this.inCart = this.storeCartData.some((ele: any) => ele.id == this.getParamValue);
    }
  }

  addCart(data: any): void {
    // Add product to cart and store it in local storage
    data['plusMinusCounter'] = 0;
    this.storeCartData.push(data);
    this.dataStorage.storeCartData(this.storeCartData);
    this.router.navigate(['/cart']); // Redirect to cart page
  }
}
