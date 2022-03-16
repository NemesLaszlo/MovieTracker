import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { parseWebAPIErrors } from 'src/app/utilities/utils';
import { SecurityService } from '../security.service';
import { userCredentials } from '../security.models'

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  errors: string[] = [];

  constructor(private securityService: SecurityService, private router: Router) { }

  ngOnInit(): void {
  }

  login(userCredentials: userCredentials) {
    this.securityService.login(userCredentials).subscribe((authenticationResponse) => {
        this.securityService.saveToken(authenticationResponse);
        this.router.navigate(['/']);
      }, error => this.errors = parseWebAPIErrors(error));
  }

}
