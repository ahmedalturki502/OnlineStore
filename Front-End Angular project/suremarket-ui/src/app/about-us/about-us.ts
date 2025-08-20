import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

interface TeamMember {
  id: number;
  name: string;
  position: string;
  bio: string;
  image: string;
  linkedin?: string;
}

interface CompanyValue {
  icon: string;
  title: string;
  description: string;
}

@Component({
  selector: 'app-about-us',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './about-us.html',
  styleUrls: ['./about-us.css']
})
export class AboutUsComponent {
  
  companyValues: CompanyValue[] = [
    {
      icon: '🎯',
      title: 'Customer First',
      description: 'We prioritize customer satisfaction and strive to exceed expectations in every interaction.'
    },
    {
      icon: '🌟',
      title: 'Quality Products',
      description: 'We curate only the finest products from trusted brands and manufacturers worldwide.'
    },
    {
      icon: '🚀',
      title: 'Innovation',
      description: 'We continuously innovate to provide the best shopping experience with cutting-edge technology.'
    },
    {
      icon: '🤝',
      title: 'Trust & Integrity',
      description: 'We build lasting relationships through honest business practices and transparent communication.'
    }
  ];

  teamMembers: TeamMember[] = [
    {
      id: 1,
      name: 'Ahmed Al-Rahman',
      position: 'Chief Executive Officer',
      bio: 'Ahmed founded Sure Market with a vision to revolutionize e-commerce in the Middle East. With over 15 years of experience in retail and technology.',
      image: 'https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=300&h=300&fit=crop&crop=face',
      linkedin: '#'
    },
    {
      id: 2,
      name: 'Sarah Mitchell',
      position: 'Chief Technology Officer',
      bio: 'Sarah leads our technology team with expertise in scalable e-commerce platforms and user experience design, ensuring our platform stays ahead of the curve.',
      image: 'https://images.unsplash.com/photo-1494790108755-2616b612b829?w=300&h=300&fit=crop&crop=face',
      linkedin: '#'
    },
    {
      id: 3,
      name: 'Mohammad Hassan',
      position: 'Head of Operations',
      bio: 'Mohammad oversees our supply chain and logistics operations, ensuring fast and reliable delivery to customers across the region.',
      image: 'https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=300&h=300&fit=crop&crop=face',
      linkedin: '#'
    },
    {
      id: 4,
      name: 'Emily Chen',
      position: 'Customer Experience Manager',
      bio: 'Emily focuses on enhancing customer satisfaction and building strong relationships through exceptional service and support.',
      image: 'https://images.unsplash.com/photo-1438761681033-6461ffad8d80?w=300&h=300&fit=crop&crop=face',
      linkedin: '#'
    }
  ];

  companyStats = [
    { number: '50K+', label: 'Happy Customers' },
    { number: '10K+', label: 'Products Available' },
    { number: '25+', label: 'Countries Served' },
    { number: '99.9%', label: 'Uptime Reliability' }
  ];

  openLinkedIn(url?: string): void {
    if (url && url !== '#') {
      window.open(url, '_blank');
    } else {
      console.log('LinkedIn profile not available');
    }
  }
}
