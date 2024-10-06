import { Component, OnInit } from '@angular/core';
import { GetDataService } from '../service/get-data.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit{
  
  bannerImgs=[
    {
      id: 1,
      img: '../../assets/Images/Banner/image.webp' 
    },
    {
      id: 2,
      img: '../../assets/Images/Banner/image.webp' 
    },
    {
      id: 3,
      img: '../../assets/Images/Banner/image.webp' 
    },
  ]

  
  constructor(private getData: GetDataService){}

  getCategorizedData:any;
  getApplianceData: any= [];
  ngOnInit(): void {
    this.getCategorizedData= this.getData.categoriesData;
    this.getData.productData.filter((ele: any)=> {
      if(ele.category == 'appliances'){
        this.getApplianceData.push(ele);
      }

    })
  }
  
}
