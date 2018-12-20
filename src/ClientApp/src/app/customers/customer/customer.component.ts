import { Component, OnInit } from '@angular/core';
import { CustomerService } from '../shared/customer.service';
import { NgForm } from '../../../../node_modules/@angular/forms';
import { ToastrService } from '../../../../node_modules/ngx-toastr';

@Component({
  selector: 'app-customer',
  templateUrl: './customer.component.html',
  styleUrls: ['./customer.component.css']
})
export class CustomerComponent implements OnInit {

  constructor(private service: CustomerService, private toastr: ToastrService) { }

  ngOnInit() {
    this.resetForm();
  }

  resetForm(form?: NgForm) {
    if (form != null) {
      form.resetForm();
    }
    this.service.formData = {
      Name: '',
      Email: ''
    };
  }

  onSubmit(form: NgForm) {
    this.insertRecord(form);
  }

  insertRecord(form: NgForm) {
    this.service.postCustomer(form.value).subscribe(res => {
      this.toastr.success('Inserted successfuly', 'Customer Added');
      this.resetForm(form);
    });
  }
}
