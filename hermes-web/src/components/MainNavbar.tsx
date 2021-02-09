import React, { useState, useReducer } from 'react';
import { Navbar, NavbarBrand, NavbarToggler, Nav, Collapse, NavLink, NavItem, Button, UncontrolledDropdown, DropdownToggle, DropdownMenu, DropdownItem, Badge } from 'reactstrap';
import { Link } from 'react-router-dom';
import { UserAPI } from '../api/UserAPI';

type MainNavbarProps = {
    items: {
        url: string,
        admin: boolean,
        label: string
    }[]
}
export function HNavbar({ items }: MainNavbarProps) {
    const rights = localStorage.getItem('hermes.rights');
    const userID = localStorage.getItem('hermes.userID');
    const profilePhotoURL = localStorage.getItem('hermes.profilePhotoURL');
    const [{ isOpen }, dispatch] = useReducer(reducer, {
        isOpen: false
    });

    function handleToggle() {
        dispatch({ _type: 'TOGGLE' });
    }
    function handleLogout() {
        UserAPI.logOut();
    }

    return (
        <Navbar className='HNavbar' color="primary" dark expand="md" fixed='top'>
            <NavbarBrand className='HNavbar-brand appBrandFont' tag={Link} to='/'>Emeenki</NavbarBrand>
            <Badge color='secondary'>beta</Badge>
            <NavbarToggler onClick={handleToggle} />
            <Collapse isOpen={isOpen} navbar>
                <Nav navbar>
                    <NavItem>
                        <NavLink className='appBrandFont text-white p-0 ml-4' href='https://discord.gg/jkNBqkN' target='_blank'>
                            Join us on Discord <img src='data:image/png;base64,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' height='33px'></img>
                        </NavLink>
                    </NavItem>
                </Nav>
                <Nav className="ml-auto" navbar>
                    { items.map((e, index) =>
                        (rights == 'admin' && e.admin || !e.admin) &&
                            <NavItem key={index}>
                                <NavLink className='appBrandFont' tag={Link} to={e.url} style={{ color: '#FFF'}}>{e.label}</NavLink>
                            </NavItem>
                    )}
                    { (userID === null) && 
                        <NavItem>
                            <NavLink className='appBrandFont' tag={Link} to='/' style={{ color: '#FFF'}}>LOG IN</NavLink>
                        </NavItem>
                    }
                    { (userID !== null) && 
                        <>
                            <UncontrolledDropdown nav inNavbar>
                                <DropdownToggle className='appBrandFont' nav  style={{ color: '#FFF'}}>{userID}</DropdownToggle>
                                <DropdownMenu right>
                                    <DropdownItem tag={Link} to={`/profile/${userID}`}>User Profile</DropdownItem>
                                    <DropdownItem onClick={handleLogout}>Log Out</DropdownItem>
                                </DropdownMenu>
                            </UncontrolledDropdown>
                            <NavItem>
                                <NavLink className='HNavbar-link' style={{ background: `url("${profilePhotoURL}") center center` }}>
                                </NavLink>
                            </NavItem>
                        </>
                    }
                </Nav>
            </Collapse>
        </Navbar>
    );
}

type State = {
    isOpen: boolean;
}
type Action =
| { _type: 'TOGGLE' }
function reducer(state: State, action: Action) : State {
    switch (action._type) {
        case 'TOGGLE': return { ...state, isOpen: !state.isOpen };
    }
}