import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GetDataService } from '../service/get-data.service';

@Component({
  selector: 'app-product-page',
  templateUrl: './product-page.component.html',
  styleUrls: ['./product-page.component.css']
})
export class ProductPageComponent implements OnInit {

  getParamValue: any;
  getProductData: any = [];
  filterProductData: any = [];
  subcategoriesProductData: any = [];
  constructor(private route: ActivatedRoute, private get: GetDataService) { }

  ngOnInit(): void {
    this.getParamValue = this.route.snapshot.paramMap.get('name');


    this.get.productData.filter((ele: any) => {
      if (ele.category === this.getParamValue) {
        this.getProductData.push(ele);
        this.filterProductData.push(ele);

      }
    }
    );
    this.get.subcategoryData.filter(
      (ele: any) => {
        if (ele.categories == this.getParamValue) {
          this.subcategoriesProductData.push(ele);
        }
      }
    )
  }
  filterSelect(data: any) {
    var getFilterValue: any = data.target.value;
    console.log(getFilterValue);

    if (getFilterValue != 'all') {
      this.get.productData.filter((ele: any) => {
        if (ele.subcategory == getFilterValue) {
          this.filterProductData.push(ele);
        }
      });
    }
    else {
      this.filterProductData = this.getProductData;
    }
  }
}