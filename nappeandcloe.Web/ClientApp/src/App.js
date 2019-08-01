import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import Inventory from './components/Inventory';
import AddProduct from './components/AddProduct';
import Product from './components/Product';
import Customers from './components/Customers';
import AddCustomer from './components/AddCustomer';
import Customer from './components/Customer';
import NewOrder from './components/NewOrder';
import ViewDay from './components/ViewDay';

export default class App extends Component {
  displayName = App.name

  render() {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/Inventory' component={Inventory} />
        <Route path='/AddProduct' component={AddProduct} />
        <Route path='/ViewProduct/:id' component={Product} />
        <Route path='/Customers' component={Customers} />
        <Route path='/AddCustomer' component={AddCustomer} />
        <Route path='/Customer/:id' component={Customer} />
        <Route path='/NewOrder' component={NewOrder} />
        <Route path='/ViewDay/:month/:day/:year' component={ViewDay} />
      </Layout>
    );
  }
}
