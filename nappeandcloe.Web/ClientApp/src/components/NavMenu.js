import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Glyphicon, Nav, Navbar, NavItem } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';
import './NavMenu.css';

export class NavMenu extends Component {
  displayName = NavMenu.name

  render() {
    return (
      <Navbar inverse fixedTop fluid collapseOnSelect>
        <Navbar.Header>
          <Navbar.Brand>
            <Link to={'/'}>NappeAndCloe</Link>
          </Navbar.Brand>
          <Navbar.Toggle />
        </Navbar.Header>
        <Navbar.Collapse>
          <Nav>
            <LinkContainer to={'/'} exact>
              <NavItem>
                <Glyphicon glyph='calendar' /> Home
              </NavItem>
            </LinkContainer>
            <LinkContainer to={'/newOrder'}>
              <NavItem>
                <Glyphicon glyph='shopping-cart' /> New Order
              </NavItem>
            </LinkContainer>
            <LinkContainer to={'/inventory'}>
              <NavItem>
                <Glyphicon glyph='th' /> My Inventory
              </NavItem>
            </LinkContainer>
            <LinkContainer to={'/customers'}>
              <NavItem>
                <Glyphicon glyph='user' /> My Customers
              </NavItem>
            </LinkContainer>
          </Nav>
        </Navbar.Collapse>
      </Navbar>
    );
  }
}
