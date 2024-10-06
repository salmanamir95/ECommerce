import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class GetDataService {

  constructor() { }
  //sub-category

  subcategoryData= [
    {id: 1, categories: 'fashion', subcategories: 'shoes'},
    {id: 2, categories: 'fashion', subcategories: 'shoes'}
  ];
  
  
  //category
  categoriesData = [
    {
      id: 1,
      name: 'Fashion',
      img: '../../assets/Images/Categories/Fashion/test.jpg',
      code: 'fashion'
    },
    {
      id: 2,
      name: 'Fashion',
      img: '../../assets/Images/Categories/Fashion/test.jpg',
      code: 'fashion'
    },
    {
      id: 3,
      name: 'Fashion',
      img: '../../assets/Images/Categories/Fashion/test.jpg',
      code: 'fashion'
    },
  ];

  productData = [
    {
      id: 1,
      img: '../../assets/Shoes/product.jpg',
      category: 'fashion',
      price: 500
    },
    {
      id: 2,
      img: '../../assets/Shoes/product.jpg',
      category: 'fashion',
      price: 1500
    },
    {
      id: 3,
      img: '../../assets/Shoes/product.jpg',
      category: 'fashion',
      price: 2500
    }
  ]
}
