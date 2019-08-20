import React, { Component } from 'react';
import axios from 'axios';
import { Link } from 'react-router-dom';
import produce from 'immer'
import Loader from 'react-loader-spinner';



export default class Inventory extends Component {
    state={
        products : [],
        productLabels : [],
        labels : [],
        sizes: [],

        searchContent: '',
        searchId: '',
        searchSizeId: '',

        loading: true,
        
        tags: [],
        editSizesMode: ''
    }

    

    componentDidMount = () => {

        this.setProducts();
        this.setLabels();
        this.setSizes();
        

    }
    setLabels = () => {

        axios.get('/api/product/getalllabels').then(({ data }) => {
            
            this.setState({ labels: data });
        });
    }

    setSizes = () => {

        axios.get('/api/product/getallSizes').then(({ data }) => {
            
            this.setState({ sizes: data });
        });
    }


    setProducts = () => {


        axios.get('/api/product/getallproducts').then(({ data }) => {
            
            this.setState({ products: data });

            let {products, searchContent, searchId, searchSizeId, labels, sizes} = this.state;

            if (searchId){
                
                products = products.filter(p => p.productLabels.some(l => l.labelId === searchId));
              }

            if (searchSizeId){
                
                products = products.filter(p => p.productSizes.some(l => l.sizeId === searchSizeId));
             }

            products = products.filter(p => p.name.toLowerCase().includes(searchContent.toLowerCase()));


            this.setState({products, loading: false});
        });

    }
    

   
    tagClicked = (id) => {
        this.setState({searchId: id});
            this.setProducts();
    }

    sizeClicked = (id) => {
        if (!this.state.editSizesMode){
            this.setState({searchSizeId: id});
            this.setProducts();
        }
       
    }

    search = (e) => {

        this.setState({searchContent: e.target.value})
        this.setProducts();

    }

    viewProduct = (id) => {
        this.props.history.push(`/viewProduct/${id}`)
    }

    updateSizes = () => {
        const {sizes} = this.state;
        axios.post(`/api/product/updateSizes`, {sizes});
        this.setState({editSizesMode: false})
    }

    editSizes = () => {
        this.setState({editSizesMode: true})
    }

    changeSize = (e, id) => {
        const nextState = produce(this.state, draft => {
            draft.sizes.find(s => s.id === id).name = e.target.value;
        });
        this.setState(nextState);
    }

    render() {

        const {products, labels, searchContent, searchId, loading, sizes, searchSizeId, editSizesMode} = this.state;
        const {tagClicked, search, viewProduct, sizeClicked, changeSize, updateSizes, editSizes} = this;

        
        const tagStyle = {
            margin : 5, 
            size: 10,
            cursor : 'pointer'
        }

        const divStyle = {
            marginTop : 25
        }
       
        let content = '';
        if (loading){
            content = (
                <div className="container" style={{textAlign: 'center', marginTop: '100px'}}>
                    <Loader
                type="Puff"
                color="#00BFFF"
                height="100"
                width="100"
            />   
            <h2>Loading..</h2>
                </div>
            )
        }
        else{
            content = (
                <div>
                <div className="container">
                    <div className='row'>
                    <div className='col-md-3'>
                            <div style={{marginTop: 25, textAlign: 'center'}}>
                                <label>Sizes</label>
                            </div>
                            <hr />
                            <div onClick={() => {sizeClicked()}} style={{border: '1px solid', margin: 15, padding: 5, textAlign: 'center', borderRadius: '5px', cursor: 'pointer'}}>
                                <h4>View All Sizes</h4>
                            </div>
                        {sizes.filter(l => products.some(p => p.productSizes.some(s => s.sizeId === l.id)))
                            .map(s => <div key={s.id} onClick={() => {sizeClicked(s.id)}} style={{border: '1px solid', margin: 15, padding: 5, textAlign: 'center', borderRadius: '5px', cursor: 'pointer'}}>
                                {editSizesMode ? 
                            <input style={{margin: 5, width: '90%'}} type='text' value={s.name} className='form-control' placeholder='Size' onChange={(e) => {changeSize(e, s.id)}} />
                                : 
                            searchSizeId === s.id ? <h3>{s.name}</h3> : <h4>{s.name}</h4>
                            }
                            </div>)}
                            <div className='col-md-12'>
                                {!editSizesMode ? 
                                    <button className='btn btn-sm btn-block btn-warning' onClick={editSizes}>Edit Sizes</button>
                                        :
                                    <button className='btn btn-sm btn-block btn-success' onClick={updateSizes}>Update Sizes</button>
                                }
                            </div>
                    </div>
                    <div className="col-md-6">
                        <div className='row'>
                            <div style={divStyle} className="col-md-12">
                                 <Link to='/addproduct'>
                                       <button className="btn btn-block btn-primary">Add Product</button>
                                 </Link>
                             </div>
                            <div>
                                <div className="col-md-12" style={divStyle}>
                                    <input type="text" placeholder="Search name..." className="form-control" onChange={search} value={searchContent} />
                                </div>
                                <div className="col-md-12">
                                    <div className="col-md-1 col-md-offset-11">
                                            <h4>{products.length}</h4>
                                    </div>
                                </div>
                            </div>
                       </div>
                       
                   
                    <div style={{textAlign: 'center'}}>
                            {products.map((p) => (
                                <div key={p.id} className="row well" onClick={() => viewProduct(p.id)} style={{cursor: 'pointer'}}>
                                    <div className="col-md-3" style={{margin: 5, textAlign: 'center'}}>
                                            <img src={`/UploadedImages/${p.pictureName}`} alt="pic" className="img-responsive" style={{maxHeight: '75px', borderRadius: '50%'}} />
                                    </div>
                                    <div className="col-md-6 offset-md-4" style={{margin: 5, textAlign: 'center', marginLeft: 80}}>
                                
                                            <h3>{p.name}</h3>
                                    </div>
                                </div>
                            )
                            )}
                    </div>
                </div>
                <div className='col-md-3'>
                  <div>
                        <div style={{marginTop: 25, textAlign: 'center'}} className="col-md-12">
                            <div style={{textAlign: 'center'}}>
                                <label>Labels</label>
                            </div>
                            <hr />
                            <h1 style={tagStyle} onClick={() => {tagClicked()}} className={searchId ? "btn btn-info sm" : "btn btn-info"}>View All</h1>
                                {labels.filter(l => products.some(p => p.productLabels.some(s => s.labelId === l.id)))
                                    .map(l => <h1 style={tagStyle} onClick={() => {tagClicked(l.id)}} className={searchId === l.id ? "btn btn-primary" : "btn btn-info"} key={l.id}>{l.name}</h1>)}
                        </div>
                   </div>
                </div>
                </div>
                </div>
                </div>)
        }

        return (
            <div>
                <div className='row' style={{margin: 15}}>
                    <button onClick={() => this.props.history.goBack()} className='btn btn-sm btn-primary'>Back</button>                     
               </div>
                {content}
            </div>
        )
    }
}
