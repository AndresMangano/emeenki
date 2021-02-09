import React from 'react';
import { SocialMediaWeb } from "./SocialMedia"
import { Navbar, NavItem, Nav, NavbarBrand } from 'reactstrap';

export function Footer() {
     return (
        <Navbar className="Footer">
            <br></br>
            <NavbarBrand>
                <h1 className="BrandFooter"><b>Emeenki</b></h1> 
                <h6>Connecting the world through language learning</h6> 
            </NavbarBrand>
            <Nav>
                <NavItem className="FooterList">
                    <ul>
                        <li>Developed by hoxoN Studios</li>
                        <li>Contact: Emeenki@hotmail.com</li>
                    </ul>
                </NavItem>
            </Nav>
            <Nav>
                <NavItem>
                    <h3><strong>Follow us</strong></h3> 
                    <SocialMediaWeb/>
                </NavItem>
            </Nav>
            <br></br>
        </Navbar>      
    );
}
