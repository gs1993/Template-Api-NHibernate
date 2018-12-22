export class CustomerFormData {
    Name: string;
    Email: string;
    Id?: number;

    constructor(name: string, email: string, id?: number) {
        this.Id = id;
        this.Name = name;
        this.Email = email;
    }
}

export class Customer {
    Id: number;
    Name: string;
    Email: string;
    Status: string;
    StatusExpirationDate: string;
    MoneySpent: number;

    constructor(customer: any) {
        this.Id = customer.id;
        this.Name = customer.name;
        this.Email = customer.email;
        this.Status = customer.status;
        this.StatusExpirationDate = customer.statusExpirationDate;
        this.MoneySpent = customer.moneySpent;
    }
}
