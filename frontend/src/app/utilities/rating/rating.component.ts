import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { SecurityService } from 'src/app/security/security.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-rating',
  templateUrl: './rating.component.html',
  styleUrls: ['./rating.component.css']
})
export class RatingComponent implements OnInit {
  @Input() maxRating: number = 5;
  @Input() selectedRate: number = 0;

  previousRate: number = 0;
  maxRatingArr: number[] = [];

  @Output() onRating: EventEmitter<number> = new EventEmitter<number>();
  
  constructor(private securityService: SecurityService) { }

  ngOnInit(): void {
    this.maxRatingArr = Array(this.maxRating).fill(0);
  }

  handleMouseEnter(index: number){
    this.selectedRate = index + 1;
  }

  handleMouseLeave(){
    if (this.previousRate !== 0){
      this.selectedRate = this.previousRate;
    } else{
      this.selectedRate = 0;
    }
  }

  rate(index: number){
    if (this.securityService.isAuthenticated()){
      this.selectedRate = index+1;
      this.previousRate = this.selectedRate;
      this.onRating.emit(this.selectedRate);
    } else{
      Swal.fire("Error", "You need to log in before voting", "error");
    }
  }

}
