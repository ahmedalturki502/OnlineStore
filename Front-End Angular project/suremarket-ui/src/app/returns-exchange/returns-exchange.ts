import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

interface PolicySection {
  id: string;
  title: string;
  description: string;
  items: string[];
  icon: string;
}

@Component({
  selector: 'app-returns-exchange',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './returns-exchange.html',
  styleUrls: ['./returns-exchange.css']
})
export class ReturnsExchangeComponent {
  
  policySections: PolicySection[] = [
    {
      id: 'eligibility',
      title: 'Return Eligibility',
      description: 'Items must meet the following criteria to be eligible for return:',
      icon: '✓',
      items: [
        'Items must be returned within 30 days of purchase',
        'Products must be in original, unused condition',
        'All original packaging and tags must be included',
        'Items must not be damaged by normal wear and tear',
        'Custom or personalized items are not eligible for return',
        'Digital products and gift cards are non-returnable'
      ]
    },
    {
      id: 'process',
      title: 'Return Process',
      description: 'Follow these simple steps to return your item:',
      icon: '📋',
      items: [
        'Log into your account and go to "Order History"',
        'Select the order containing the item you wish to return',
        'Click "Request Return" and select the reason for return',
        'Print the prepaid return shipping label',
        'Package the item securely with all original materials',
        'Drop off at any authorized shipping location'
      ]
    },
    {
      id: 'timeframes',
      title: 'Processing Timeframes',
      description: 'Expected timeframes for different stages of the return process:',
      icon: '⏰',
      items: [
        'Return request approval: 1-2 business days',
        'Shipping time: 3-7 business days (depending on location)',
        'Inspection and processing: 2-3 business days',
        'Refund processing: 3-5 business days',
        'Credit card refunds: 5-10 business days',
        'Store credit: Instant upon approval'
      ]
    },
    {
      id: 'exchanges',
      title: 'Exchange Policy',
      description: 'Information about exchanging items for different sizes or colors:',
      icon: '🔄',
      items: [
        'Exchanges are available for size and color variations',
        'Original item must be in returnable condition',
        'Price differences will be charged or refunded accordingly',
        'Exchange requests must be made within 30 days',
        'Free exchange shipping for defective items',
        'Customer pays shipping for preference-based exchanges'
      ]
    },
    {
      id: 'refunds',
      title: 'Refund Information',
      description: 'Details about how refunds are processed:',
      icon: '💰',
      items: [
        'Refunds are issued to the original payment method',
        'Shipping costs are non-refundable (unless item was defective)',
        'Partial refunds may apply for items not in original condition',
        'Store credit option available for faster processing',
        'International orders: customer pays return shipping',
        'Refund amount excludes any promotional discounts'
      ]
    },
    {
      id: 'exceptions',
      title: 'Special Conditions',
      description: 'Items and situations with special return conditions:',
      icon: '⚠️',
      items: [
        'Electronics: Must include all accessories and original packaging',
        'Clothing: Must be unworn with tags attached',
        'Perishable goods: Not eligible for return',
        'Final sale items: Clearly marked and non-returnable',
        'Damaged during shipping: Report within 48 hours',
        'Wrong item shipped: Free return and exchange'
      ]
    }
  ];

  scrollToSection(sectionId: string): void {
    const element = document.getElementById(sectionId);
    if (element) {
      element.scrollIntoView({ behavior: 'smooth' });
    }
  }
}
