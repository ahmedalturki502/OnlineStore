import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';

interface ContactInfo {
  icon: string;
  title: string;
  details: string[];
  action?: string;
}

@Component({
  selector: 'app-contact-us',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './contact-us.html',
  styleUrls: ['./contact-us.css']
})
export class ContactUsComponent implements OnInit {
  contactForm!: FormGroup;
  isSubmitting = false;
  isSubmitted = false;

  contactInfo: ContactInfo[] = [
    {
      icon: '📧',
      title: 'Email Us',
      details: [
        'General: info@suremarket.com',
        'Support: support@suremarket.com',
        'Sales: sales@suremarket.com'
      ],
      action: 'mailto:info@suremarket.com'
    },
    {
      icon: '📞',
      title: 'Call Us',
      details: [
        'Main: +966 11 123 4567',
        'Support: +966 11 123 4568',
        'Mon-Fri 9AM-6PM'
      ],
      action: 'tel:+966111234567'
    },
    {
      icon: '📍',
      title: 'Visit Us',
      details: [
        'Sure Market Headquarters',
        '123 Business District',
        'Riyadh 12345, Saudi Arabia'
      ],
      action: 'https://maps.google.com'
    },
    {
      icon: '💬',
      title: 'Live Chat',
      details: [
        'Available 24/7',
        'Instant responses',
        'Technical support'
      ],
      action: 'chat'
    }
  ];

  businessHours = [
    { day: 'Monday - Friday', hours: '9:00 AM - 6:00 PM' },
    { day: 'Saturday', hours: '10:00 AM - 4:00 PM' },
    { day: 'Sunday', hours: 'Closed' }
  ];

  constructor(private formBuilder: FormBuilder) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  private initializeForm(): void {
    this.contactForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]],
      subject: ['', [Validators.required, Validators.minLength(5)]],
      message: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(1000)]]
    });
  }

  get name() {
    return this.contactForm.get('name');
  }

  get email() {
    return this.contactForm.get('email');
  }

  get subject() {
    return this.contactForm.get('subject');
  }

  get message() {
    return this.contactForm.get('message');
  }

  onSubmit(): void {
    if (this.contactForm.valid) {
      this.isSubmitting = true;
      
      // Simulate API call
      setTimeout(() => {
        console.log('Contact form submitted:', this.contactForm.value);
        // TODO: Implement actual form submission when backend is ready
        this.isSubmitting = false;
        this.isSubmitted = true;
        
        // Reset form after successful submission
        this.contactForm.reset();
        
        // Reset success message after 5 seconds
        setTimeout(() => {
          this.isSubmitted = false;
        }, 5000);
      }, 2000);
    } else {
      // Mark all fields as touched to show validation errors
      this.markFormGroupTouched();
    }
  }

  private markFormGroupTouched(): void {
    Object.keys(this.contactForm.controls).forEach(key => {
      const control = this.contactForm.get(key);
      if (control) {
        control.markAsTouched();
      }
    });
  }

  handleContactAction(action?: string): void {
    if (!action) return;

    switch (action) {
      case 'chat':
        alert('Live chat feature coming soon!');
        break;
      default:
        if (action.startsWith('http') || action.startsWith('mailto:') || action.startsWith('tel:')) {
          window.open(action, '_blank');
        }
        break;
    }
  }

  getFieldErrorMessage(fieldName: string): string {
    const field = this.contactForm.get(fieldName);
    
    if (field?.errors && field.touched) {
      if (field.errors['required']) {
        return `${this.getFieldDisplayName(fieldName)} is required`;
      }
      if (field.errors['email']) {
        return 'Please enter a valid email address';
      }
      if (field.errors['minlength']) {
        const requiredLength = field.errors['minlength'].requiredLength;
        return `${this.getFieldDisplayName(fieldName)} must be at least ${requiredLength} characters`;
      }
      if (field.errors['maxlength']) {
        const maxLength = field.errors['maxlength'].requiredLength;
        return `${this.getFieldDisplayName(fieldName)} cannot exceed ${maxLength} characters`;
      }
    }
    
    return '';
  }

  private getFieldDisplayName(fieldName: string): string {
    const displayNames: { [key: string]: string } = {
      'name': 'Name',
      'email': 'Email',
      'subject': 'Subject',
      'message': 'Message'
    };
    return displayNames[fieldName] || fieldName;
  }

  hasFieldError(fieldName: string): boolean {
    const field = this.contactForm.get(fieldName);
    return !!(field?.errors && field.touched);
  }

  getMessageCharacterCount(): number {
    return this.message?.value?.length || 0;
  }
}
