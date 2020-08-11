import React, { useState } from "react";
import { Button, Form, Container, Row, Col } from "react-bootstrap";
import { login } from '../services/auth_service';

export type LoginInputs = {
  email: string
  password: string
}

export default function Login() {
  const [inputs, setInputs] = useState({ email: "", password: "" });
  const [error, setError] = useState("");

  const handleSubmit = async (e: React.ChangeEvent<any>) => {
    e.preventDefault();

    const res = await login(inputs);
    if (res) {
      setError(res);
    }
  };

  const handleInputChange = (e: React.ChangeEvent<any>) => {
    e.persist();
    setInputs({
      ...inputs,
      [e.target.name]: e.target.value,
    });
  };

  return <>
    <Container className="h-100" style={{marginTop:'200px'}}>
      <Row className="h-100 justify-content-center align-items-center">
        <Col md={{span: 8}} lg={{span:6}}>
          <Form onSubmit={handleSubmit} className='form-signin'>

              <h1 className="text-center">Rhyze</h1>
              <Form.Group controlId="email">
                <Form.Label></Form.Label>
                <Form.Control type="email" 
                              name="email"
                              placeholder="Email Address"
                              autoFocus
                              value={inputs.email}
                              onChange={handleInputChange}
                />
              </Form.Group>
              <Form.Group controlId="password">
                <Form.Label></Form.Label>
                <Form.Control type="password"
                              name="password"
                              placeholder="Password"
                              value={inputs.password}
                              onChange={handleInputChange}
                />
              </Form.Group>
              <Button block type="submit">Login</Button>
          </Form>
          {error ? <h5>Error: {error}</h5> : null}
        </Col>
      </Row>
    </Container>
  </>;
}