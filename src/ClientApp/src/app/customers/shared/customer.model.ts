export class CustomerFormData {
    Name: string;
    Email: string;

    constructor(name: string, email: string) {
        this.Name = name;
        this.Email = email;
    }
}

export class Customer {
    id: number;
    Name: string;
    Email: string;
    Status: string;
    StatusExpirationDate: string;
    MoneySpent: number;

    constructor(customer: any) {
        this.id = customer.id;
        this.Name = customer.name;
        this.Email = customer.email;
        this.Status = customer.status;
        this.StatusExpirationDate = customer.statusExpirationDate;
        this.MoneySpent = customer.moneySpent;
    }
}
